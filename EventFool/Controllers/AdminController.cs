using EventFool.Domain;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventFool.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public ActionResult Index()
        {
                return View();
        }
        #region Category

        [Authorize]
        public ActionResult IndexCategory()
        {
            return View(_unitOfWork.Categories.ReadAll());
        }

        [Authorize]
        public ActionResult CreateCategory()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "Name")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Categories.Create(category);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            

            return View(category);
        }

        [Authorize]
        public ActionResult EditCategory(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _unitOfWork.Categories.Read(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "Id, Name")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Categories.Update(category);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return View(category);
        }

        public ActionResult DeleteCategory(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _unitOfWork.Categories.Read(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedCategory(Guid id)
        {
            try
            {
                Category category = _unitOfWork.Categories.Read(id);
                _unitOfWork.Categories.Delete(category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }
        #endregion

        #region Location
        [Authorize]
        public ActionResult IndexLocation()
        {
            return View(_unitOfWork.Locations.ReadAll());
        }


        [Authorize]
        public ActionResult CreateLocation()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLocation([Bind(Include = "Name, Longitude, Latitude, Address, City, Country, State, PostalCode")] Location location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Locations.Create(location);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(location);
        }

        [Authorize]
        public ActionResult EditLocation(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = _unitOfWork.Locations.Read(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLocation([Bind(Include = "Id, Name, Longitude, Latitude, Address, City, Country, State, PostalCode")] Location location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Locations.Update(location);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
            return View(location);
        }

        [Authorize]
        public ActionResult DeleteLocation(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = _unitOfWork.Locations.Read(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        [HttpPost, ActionName("DeleteLocation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedLocation(Guid id)
        {
            try
            {
                Location location = _unitOfWork.Locations.Read(id);
                _unitOfWork.Locations.Delete(location);
                _unitOfWork.Save();
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
            return RedirectToAction("Index");
        }
        #endregion

        #region User
        [Authorize]
        public ActionResult IndexUser()
        {
            return View(_unitOfWork.Users.ReadAll());
        }
        [Authorize]
        public ActionResult DeleteUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _unitOfWork.Users.Read(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUser(string id)
        {
            try
            {
                ApplicationUser applicationUser = _unitOfWork.Users.Read(id);
                _unitOfWork.Users.Delete(applicationUser);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }
        #endregion

        #region Ticket
        [Authorize]
        public ActionResult IndexTicket()
        {
            return View(_unitOfWork.Tickets.ReadAll());
        }
        [Authorize]
        public ActionResult EditTicket(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = _unitOfWork.Tickets.Read(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTicket([Bind(Include = "Id, EventId, UserId, QRCode, Price, Active, PayPalReference")] Ticket ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Tickets.Update(ticket);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return View(ticket);
        }
        #endregion

        #region Event
        [Authorize]
        public ActionResult IndexEvent()
        {
            return View(_unitOfWork.Events.ReadAll());
        }
        [Authorize]
        public ActionResult EditEvent(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event myEvent = _unitOfWork.Events.GetLocationCategory(id);
            if (myEvent == null)
            {
                return HttpNotFound();
            }
            AdminEditEventViewModel adminEditEventViewModel = new AdminEditEventViewModel()
            {
                Id = myEvent.Id,
                Name = myEvent.Name,
                StartDate = myEvent.StartDate,
                EndDate = myEvent.EndDate,
                MinTickets = myEvent.MinTickets,
                MaxTickets = myEvent.MaxTickets,
                TicketPrice = myEvent.TicketPrice,
                Description = myEvent.Description,
                CategoryId = myEvent.Category.Id,
                LocationId = myEvent.Location.Id
            };
            return View(adminEditEventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(AdminEditEventViewModel adminEditEventViewModel)
        {
            try
            {
                Category category = _unitOfWork.Categories.Read(adminEditEventViewModel.CategoryId);
                Location location = _unitOfWork.Locations.Read(adminEditEventViewModel.LocationId);
                Event myEvent = new Event()
                {
                    Id = adminEditEventViewModel.Id,
                    Name = adminEditEventViewModel.Name,
                    StartDate = adminEditEventViewModel.StartDate,
                    EndDate = adminEditEventViewModel.EndDate,
                    MinTickets = adminEditEventViewModel.MinTickets,
                    MaxTickets = adminEditEventViewModel.MaxTickets,
                    TicketPrice = adminEditEventViewModel.TicketPrice,
                    Description = adminEditEventViewModel.Description,
                    Category = category,
                    Location = location
                };
                if (ModelState.IsValid)
                {
                    _unitOfWork.Events.Update(myEvent);
                    _unitOfWork.Save();
                    return RedirectToAction("IndexEvent");
                }
                return View(myEvent);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
        }
        [Authorize]
        public ActionResult DeleteEvent(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event myEvent = _unitOfWork.Events.Read(id);
            if (myEvent == null)
            {
                return HttpNotFound();
            }
            return View(myEvent);
        }

        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedEvent(Guid id)
        {
            try
            {
                Event myEvent = _unitOfWork.Events.Read(id);
                _unitOfWork.Events.Delete(myEvent);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
