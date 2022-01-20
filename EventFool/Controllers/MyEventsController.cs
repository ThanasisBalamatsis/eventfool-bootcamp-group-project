using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventFool.Domain;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.Services;
using EventFool.ViewModels;
using Microsoft.AspNet.Identity;

namespace EventFool.Controllers
{
    public class MyEventsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MyEventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: MyEvents
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


        [Authorize]
        public ActionResult Created()
        {
            try
            {
                List<Category> categories = _unitOfWork.Categories.ReadAll().ToList();
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
        public ActionResult FilterCreatedEvents(FiltersViewModel data)
        {

            string userId = HttpContext.User.Identity.GetUserId();

            List<Event> firstFilterEvents = QueryService.FilterCreatedBasedOnPrice(_unitOfWork, data.Price, userId);
            List<Event> secondFilterEvents = QueryService.FilterCreatedBasedOnDate(data.Day, firstFilterEvents);
            List<Event> thirdFilterEvents = QueryService.FilterCreatedBasedOnCategory(data.CategoryId, secondFilterEvents);

            List<FilteredEventsViewModel> filteredEvents = QueryService.FindFilteredEvents(thirdFilterEvents,_unitOfWork);
            if (filteredEvents.Count() == 0)
            {
                filteredEvents = null;
            }
            return PartialView("_CreatedEventsTable", filteredEvents);
        }

        [Authorize]
        public ActionResult Attended()
        {
            try
            {
                List<Category> categories = _unitOfWork.Categories.ReadAll().ToList();
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
        public ActionResult FilterAttendedEvents(FiltersViewModel data)
        {
            
            string userId = HttpContext.User.Identity.GetUserId();
            List<Event> firstFilterEvents = QueryService.FilterAttendedBasedOnPrice(_unitOfWork, data.Price, userId);
            List<Event> secondFilterEvents = QueryService.FilterAttendedBasedOnDate(data.Day, firstFilterEvents);
            List<Event> thirdFilterEvents = QueryService.FilterAttendedBasedOnCategory(data.CategoryId, secondFilterEvents);

            List<FilteredEventsViewModel>  filteredEvents = QueryService.FindFilteredEvents(thirdFilterEvents,_unitOfWork);
            if (filteredEvents.Count() == 0)
            {
                filteredEvents = null;
            }
            return PartialView("_EventsTable", filteredEvents);
        }

    }
}