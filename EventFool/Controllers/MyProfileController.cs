using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.Services;
using EventFool.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventFool.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MyProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: MyProfile
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                string userId = HttpContext.User.Identity.GetUserId();
                ApplicationUser applicationUser = _unitOfWork.Users.GetEventsTicketsPhotos(userId);
                return View(applicationUser);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        private List<PhotoViewModel> PopulatePics(string userId)
        {
            List<PhotoViewModel> photos = new List<PhotoViewModel>();
            var photo = _unitOfWork.Photos.ReadAll(userId).ToList();
            foreach (var pic in photo)
            {
                PhotoViewModel photoViewModel = new PhotoViewModel()
                {
                    Id = pic.Id,
                    Image = pic.Image
                };
                photos.Add(photoViewModel);
            }
            return photos;
        }

        [Authorize]
        public ActionResult Update()
        {
            try
            {
                string userId = HttpContext.User.Identity.GetUserId();
                ApplicationUser loggedInUser = _unitOfWork.Users.Read(userId);

                UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel()
                {
                    Id = userId,
                    FirstName = loggedInUser.FirstName,
                    LastName = loggedInUser.LastName,
                    Address = loggedInUser.Address,
                    Birthdate = loggedInUser.Birthdate,
                    Gender = loggedInUser.Gender,
                    ProfilePhoto = loggedInUser.ProfilePhoto,
                    Photos = PopulatePics(userId)
                };

                return View(updateUserViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(UpdateUserViewModel updateUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser submittedUser = ApplicationUserService.MakeApplicationUserFromViewModel(updateUserViewModel);
                    submittedUser.ProfilePhoto = updateUserViewModel.ProfilePhoto;
                    _unitOfWork.Users.Update(submittedUser);
                    _unitOfWork.Save();
                    return RedirectToAction("Index", "Home");
                }
                return View(updateUserViewModel);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }
    }
}