using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal;
using PayPal.Api;
using PayPal.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EventFool.ViewModels;
using EventFool.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using HtmlAgilityPack;
using System.Globalization;

namespace EventfoolWeb.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        // GET: Ticket

        private readonly IUnitOfWork _unitOfWork;
        public TicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Details(Guid eventId)
        {
            try
            {
                string userId = HttpContext.User.Identity.GetUserId();
                Ticket ticket = _unitOfWork.Tickets.Read(eventId, userId);

                return View(ticket);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            
        }
        [Authorize]
        public ActionResult Cancel(Guid ticketId)
        {
            try
            {
                Ticket ticket = _unitOfWork.Tickets.Read(ticketId);
                ticket.Active = false;
                _unitOfWork.Tickets.Update(ticket);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            
        }


        [Authorize]
        public ActionResult PaymentWithPaypal(Guid eventId)
        {
            try
            {
                string userID = HttpContext.User.Identity.GetUserId();

                Event selectedEvent = _unitOfWork.Events.GetLocationCategory(eventId);
                CreateTicketViewModel createTicketViewModel = new CreateTicketViewModel()
                {
                    Id = selectedEvent.Id,
                    LocationName = selectedEvent.Location.Name,
                    Name = selectedEvent.Name,
                    StartDate = selectedEvent.StartDate,
                    EndDate = selectedEvent.EndDate,
                    Description = selectedEvent.Description,
                    TicketPrice = selectedEvent.TicketPrice
                };

                return View(createTicketViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PaymentWithPaypal(CreateTicketViewModel createTicketViewModel)
        {
            var user = _unitOfWork.Users.Read(User.Identity.GetUserId());
            var selectedEvent = _unitOfWork.Events.ReadEager(createTicketViewModel.Id);

            if (selectedEvent.Tickets.Count() + 1 > selectedEvent.MaxTickets)
            {
                return RedirectToAction("Index", "Error", new { message = "There are no more available tickets for this event" });
            }

            if (createTicketViewModel.TicketPrice == 0)
            {
                try {
                    
                    QrCodeService qrService = new QrCodeService();
                    var QRCode = $"{selectedEvent.Name}{selectedEvent.StartDate.Date.ToString("M-dd-yy", CultureInfo.InvariantCulture)}.png";
                    Ticket ticket = new Ticket()
                    {
                        EventId = createTicketViewModel.Id,
                        UserId = User.Identity.GetUserId(),
                        Price = createTicketViewModel.TicketPrice,
                        PayPalReference = "Free Event",
                        QRCode = QRCode,
                        Active = true

                    };
                    var qrText = $"{createTicketViewModel.Name} \nStart Date:{createTicketViewModel.StartDate}" +
                    $"\nLocation:{createTicketViewModel.LocationName} \n FullName : {user.FirstName} {user.LastName} ";


                    qrService.GenerateCode(qrText, User.Identity.Name, ticket.QRCode);
                    _unitOfWork.Tickets.Create(ticket);
                    ticket.Event = _unitOfWork.Events.Read(createTicketViewModel.Id);
                    ticket.User = _unitOfWork.Users.Read(User.Identity.GetUserId());
                    _unitOfWork.Save();
                    return RedirectToAction("Success", "Ticket");
                }
                catch
                {
                    return RedirectToAction("Index", "Error", new { message = "You have already bought a ticket for this event" });
                }
            }

            string description = "";
            float tax = 0;
            float shipping = 0;
            //string userId = HttpContext.User.Identity.GetUserId();

            //var ticketUser = _unitOfWork.Tickets.GetEventUser(EventId, userId);
            //var model = _unitOfWork.Tickets.GetEventUser(model.EventId, model.UserId);  
            var price = createTicketViewModel.TicketPrice;

            var EventStartDate = createTicketViewModel.StartDate;
            var EventName = createTicketViewModel.Name;


            var viewData = new PayPalViewData();
            var guid = Guid.NewGuid().ToString();

            ViewBag.Id = createTicketViewModel.Id;
            var paymentInit = new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        amount = new Amount
                        {
                            currency = "USD",
                            total = (price + tax + shipping).ToString(),
                            details = new Details
                            {
                                subtotal = price.ToString(),
                                tax = tax.ToString(),
                                shipping = shipping.ToString()


                            }
                        },
                        description = description,

                         item_list = new ItemList()
                            {
                                items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        description = $"{EventName} for {EventStartDate:dddd, dd MMMM yyyy}",
                                        currency = "USD",
                                        quantity = "1",
                                        price = price.ToString(),
                                        name = createTicketViewModel.LocationName

                                    }
                                }
                            }
                    }
                },
                redirect_urls = new RedirectUrls
                {

                    return_url = Url.Action("Success", "Ticket", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Error", "Ticket", null, Request.Url.Scheme)
                    //return_url = /*Utilities.ToAbsoluteUrl(HttpContext, String.Format("~/paypal/confirmed?id={0}", guid))*/,
                    //cancel_url = /*Utilities.ToAbsoluteUrl(HttpContext, String.Format("~/paypal/canceled?id={0}", guid))*/,
                },
            };

            viewData.JsonRequest = JObject.Parse(paymentInit.ConvertToJson()).ToString(Formatting.Indented);
            
            try
            {
                var accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["clientId"], ConfigManager.Instance.GetProperties()["clientSecret"]).GetAccessToken();
                var apiContext = new APIContext(accessToken);
                var createdPayment = paymentInit.Create(apiContext);

                user = _unitOfWork.Users.Read(User.Identity.GetUserId());
                selectedEvent = _unitOfWork.Events.Read(createTicketViewModel.Id);
                QrCodeService qrService = new QrCodeService();
                var QRCode= $"{selectedEvent.Name}{selectedEvent.StartDate.Date.ToString("M-dd-yy", CultureInfo.InvariantCulture)}.png";
                Ticket ticket = new Ticket()
                {
                    EventId = createTicketViewModel.Id,
                    UserId = User.Identity.GetUserId(),
                    Price = createTicketViewModel.TicketPrice,
                    PayPalReference = createdPayment.id,
                    QRCode = QRCode,
                    Active = true

                };
                var qrText = $"{createTicketViewModel.Name} \nStart Date:{createTicketViewModel.StartDate}" +
                $"\nLocation:{createTicketViewModel.LocationName} \n FullName : {user.FirstName} {user.LastName} ";


                qrService.GenerateCode(qrText, User.Identity.Name, ticket.QRCode);
                _unitOfWork.Tickets.Create(ticket);
                ticket.Event = _unitOfWork.Events.Read(createTicketViewModel.Id);
                ticket.User = _unitOfWork.Users.Read(User.Identity.GetUserId());
                _unitOfWork.Save();

                //return RedirectToAction("Success", "Ticket", ticket);
                var approvalUrl = createdPayment.links.ToArray().FirstOrDefault(f => f.rel.Contains("approval_url"));

                if (approvalUrl != null)
                {
                    Session.Add(guid, createdPayment.id);

                    return Redirect(approvalUrl.href);
                }

                viewData.JsonResponse = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);

                return View("Error", viewData);
            }
            catch (Exception )
            {


                //string message ;
                
                //viewData.ErrorMessage = ex.Message;
                return RedirectToAction("Index", "Error", new {  message = "You have already bought this ticket for this event" });
                
            }
        }
        [Authorize]
        public ActionResult Success()
        {
            try
            {
                Ticket ticket = _unitOfWork.Tickets.ReadLast(User.Identity.GetUserId());
                return View(ticket);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportHTML(string ExportData)
        {


            HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(ExportData);
            ExportData = doc.DocumentNode.OuterHtml;
            using (MemoryStream stream = new System.IO.MemoryStream())
            {


                StringReader reader = new StringReader(ExportData);
                Document PdfFile = new Document(PageSize.LETTER);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                PdfFile.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
                PdfFile.Close();
                return File(stream.ToArray(), "application/pdf", "ExportData.pdf");

            }


        }

    }
}