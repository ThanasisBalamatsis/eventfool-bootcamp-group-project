using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventFool.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            List<Event> trendingEvents = _unitOfWork.Events.FindTrendingEvents();
            TrendingEventsViewModel trendingEventsViewModel = new TrendingEventsViewModel()
            {
                TrendingEvents = trendingEvents
            };
            return View(trendingEventsViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}