using EventFool.Repository.Interfaces;
using EventFool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventFool.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ErrorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Error
        public ActionResult Index(string message)
        {
            ErrorMessageViewModel errorMessageViewModel = new ErrorMessageViewModel()
            {
                Message = message
            };
            return View(errorMessageViewModel);
        }
    }
}