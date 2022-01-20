using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventFool.Repository;
using EventFool.Repository.Interfaces;
using EventFool.Domain.Models;
using EventFool.Models;
using EventFool.ViewModels;
using System.Net;
using EventFool.Services;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace EventFool.Controllers
{
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            try
            {
                List<Category> categories = _unitOfWork.Categories.ReadAll().ToList();
                EventCreateViewModel eventCreateViewModel = new EventCreateViewModel()
                {
                    Categories = categories,
                    Photos = PhotoService.GetPhotos(_unitOfWork, User.Identity.GetUserId()),
                    ProfilePhoto = "unknown.png"
                    //Picture = photo
                };
                return View(eventCreateViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventCreateViewModel eventCreateViewModel)
        {
            try
            {
                var eventPhotos = eventCreateViewModel.Photos.Where(x => x.Checked).Select(x => x.Id).ToList();

                if (ModelState.IsValid)
                {
                    Event myEvent = EventService.MakeEventFromViewModel(eventCreateViewModel, _unitOfWork);
                    string userID = HttpContext.User.Identity.GetUserId();

                    List<Event> allEventsFromDB = _unitOfWork.Events.ReadAll().ToList();

                    foreach (Event eventDB in allEventsFromDB)
                    {
                        if (myEvent.Location.Latitude == eventDB.Location.Latitude && 
                            myEvent.Location.Longitude == eventDB.Location.Longitude && 
                            myEvent.StartDate == eventDB.StartDate)
                        {
                            eventCreateViewModel.Categories = _unitOfWork.Categories.ReadAll().ToList();
                            eventCreateViewModel.ErrorMessage = "An event at the same location and time already exists.";
                            return View(eventCreateViewModel);
                        }
                    }

                    _unitOfWork.Events.Create(myEvent);
                    _unitOfWork.Events.Create(myEvent, userID);
                    foreach (var pic in eventPhotos)
                    {
                        _unitOfWork.Events.Create(myEvent.Id, _unitOfWork.Photos.Read(pic));
                    }
                    _unitOfWork.Save();


                    return RedirectToAction("Index", "Home");
                }

                eventCreateViewModel.Categories = _unitOfWork.Categories.ReadAll().ToList();

                return View(eventCreateViewModel);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return RedirectToAction("Index", "Error");
            }
            
        }

        [Route("Event/Update/UpdateForm")]
        [Authorize]
        public ActionResult Update(Guid eventId)
        {
            try
            {
                List<Category> categories = _unitOfWork.Categories.GetCategories().ToList();
                Event eventToUpdate = _unitOfWork.Events.GetLocationCategory(eventId);
                UpdateEventViewModel updateEventViewModel = new UpdateEventViewModel()
                {
                    Id = eventToUpdate.Id,
                    Name = eventToUpdate.Name,
                    Description = eventToUpdate.Description,
                    TicketPrice = eventToUpdate.TicketPrice,
                    MinTickets = eventToUpdate.MinTickets,
                    MaxTickets = eventToUpdate.MaxTickets,
                    LocationId = eventToUpdate.Location.Id,
                    LocationName = eventToUpdate.Location.Name,
                    Address = eventToUpdate.Location.Address,
                    Latitude = eventToUpdate.Location.Latitude,
                    Longitude = eventToUpdate.Location.Longitude,
                    CategoryId = eventToUpdate.Category.Id,
                    Categories = categories,
                    ProfilePhoto = eventToUpdate.ProfilePhoto,
                    StartDate = eventToUpdate.StartDate,
                    EndDate = eventToUpdate.EndDate,
                    Photos = PhotoService.GetPhotos(_unitOfWork, User.Identity.GetUserId(), eventId)

                };

                return View(updateEventViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Event/Update/UpdateForm")]
        public ActionResult Update(UpdateEventViewModel updateEventViewModel)
        {
            try
            {
                if (updateEventViewModel.Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var eventPhotos = updateEventViewModel.Photos.Where(x => x.Checked).Select(x => x.Id).ToList();

                if (ModelState.IsValid)
                {
                    Event myEvent = EventService.MakeEventFromViewModel(updateEventViewModel, _unitOfWork);
                    _unitOfWork.Events.Update(myEvent);
                    foreach (var pic in eventPhotos)
                    {
                        _unitOfWork.Events.Create(myEvent.Id, _unitOfWork.Photos.Read(pic));
                    }
                    _unitOfWork.Save();
                    return RedirectToAction("Created", "MyEvents");
                }
                return View(updateEventViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        [Authorize]
        [Route("Event/Details/profile-form")]
        public ActionResult Details(Guid eventId)
        {
            try
            {
                Event myEvent = _unitOfWork.Events.Read(eventId);
                var profileEventDetails = EventService.MakeViewModelFromEvent(myEvent, _unitOfWork);
                return View(profileEventDetails);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        [Authorize]
        [Route("Event/Details/private-profile")]
        public ActionResult DetailsPrivate(Guid eventId)
        {
            try
            {
                Event myEvent = _unitOfWork.Events.Read(eventId);
                var profileEventDetails = EventService.MakeViewModelFromEvent(myEvent, _unitOfWork);

                return View(profileEventDetails);
            }
            catch (Exception)
            {


                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult Search()
        {
            try
            {
                var categories = _unitOfWork.Categories.ReadAll().ToList();
                FiltersViewModel filtersViewModel = new FiltersViewModel()
                {
                    Categories = categories
                };
                return View(filtersViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        [HttpGet]
        public JsonResult GetLocations(EventSearchViewModel searchEvent)
        {

            var returnedLocations = new List<FilteredLocationsViewModel>();
            var boundLocations = LocationService.CompareBounds(_unitOfWork, searchEvent.swLat, searchEvent.swLng,
                    searchEvent.neLat, searchEvent.neLng);

            foreach (var location in boundLocations)
            {
                FilteredLocationsViewModel filteredLocations = new FilteredLocationsViewModel();
                List<AssociatedEventsViewModel> associatedEvents = new List<AssociatedEventsViewModel>();
                int eventsCounter = 0;

                foreach (Event myEvent in location.Events)
                {
                    if (QueryService.FilterEventBasedOnDropdowns(myEvent, searchEvent.Day, searchEvent.Price, searchEvent.CategoryId))
                    {
                        eventsCounter++;
                        AssociatedEventsViewModel associatedEventsViewModel = new AssociatedEventsViewModel()
                        {
                            EventId = myEvent.Id,
                            Name = myEvent.Name,
                            StartDate = myEvent.StartDate,
                            ProfilePhoto = myEvent.ProfilePhoto,
                            TicketPrice = myEvent.TicketPrice,
                            UserName = _unitOfWork.Events.ReadEager(myEvent.Id).Users.FirstOrDefault().UserName
                        };

                        associatedEvents.Add(associatedEventsViewModel);
                    }
                }
                
                if (eventsCounter != 0)
                {
                    filteredLocations.LocationId = location.Id;
                    filteredLocations.Price = searchEvent.Price;
                    filteredLocations.Day = searchEvent.Day;
                    filteredLocations.CategoryId = searchEvent.CategoryId;
                    filteredLocations.Latitude = location.Latitude;
                    filteredLocations.Longitude = location.Longitude;
                    filteredLocations.EventNumber = eventsCounter;
                    filteredLocations.AssociatedEvents = associatedEvents;

                    returnedLocations.Add(filteredLocations);
                }

            }
            return Json(returnedLocations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAssociatedEvents (List<AssociatedEventsViewModel> AssociatedEvents)
        {

            var returnedEvents = new List<AssociatedEventsViewModel>();

            foreach (var Event in AssociatedEvents)
            {
                
                    var associatedEvent = new AssociatedEventsViewModel
                    {
                        EventId = Event.EventId,
                        Name = Event.Name,
                        StartDate = _unitOfWork.Events.Read(Event.EventId).StartDate,
                        ProfilePhoto = Event.ProfilePhoto,
                        TicketPrice = Event.TicketPrice,
                        UserName = Event.UserName
                    };
                    returnedEvents.Add(associatedEvent);
                
           }
           return PartialView("_EventsPartial",returnedEvents);
        }

        [Authorize]
        public ActionResult AllEvents()
        {
            IEnumerable<Event> events = _unitOfWork.Events.ReadAllEager();
            return View(events.ToList());
        }

        [Authorize]
        public ActionResult Delete(Event eventToDelete)
        {
            try
            {
                eventToDelete = _unitOfWork.Events.Read(eventToDelete.Id);
                _unitOfWork.Events.Delete(eventToDelete);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
        }
    }
}