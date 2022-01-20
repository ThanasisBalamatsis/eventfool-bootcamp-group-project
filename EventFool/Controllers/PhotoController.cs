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
using System.Data;
using Microsoft.AspNet.Identity;
using EventFool.Domain;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using EventFool.Services;
using System.Net;

namespace EventFool.Controllers
{
    public class PhotoController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public PhotoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Photo
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }
        [Authorize]
        public ActionResult PhotosCreated() // epistrefei view me oles tis photos pou exoun dimiourgithei sti vasi
        {
            try
            {
                IEnumerable<Photo> photos = _unitOfWork.Photos.ReadAll(User.Identity.GetUserId());
                return View(photos);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error");
            }
            
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] files)
        {
            if (Request.IsAuthenticated)
            {
            //iterating through multiple file collection   
                SavePhoto(files,User.Identity.Name,User.Identity.GetUserId());
                try
                {
                    if (ModelState.IsValid)
                    {
                        _unitOfWork.Save();
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult EventProfilePicture()
        {
            
            return PartialView();
        }

        [Authorize]
        public ActionResult PhotoProfile()
        {
            string pic = _unitOfWork.Users.Read(User.Identity.GetUserId()).ProfilePhoto;
            if (pic == null)
            {
                pic = "unknown.png";
            }
            ProfPicViewModel profPicViewModel = new ProfPicViewModel()
            {
                ProfPic = pic
            };
            return PartialView("_ProfPic", profPicViewModel);
        }


        private void SavePhoto(HttpPostedFileBase[] files,string userName,string userId)
         {
            foreach (HttpPostedFileBase file in files)
            {
                //Checking file is available to save.  
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Uploads/") + userName + "/");
                    if (!Directory.Exists(ServerSavePath))
                    {
                        Directory.CreateDirectory(ServerSavePath);
                    }
                    ServerSavePath += InputFileName;
                    //Save file to server folder  
                    file.SaveAs(ServerSavePath);
                    ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    //string userID = HttpContext.User.Identity.GetUserId();
                    var user = _unitOfWork.Users.Read(userId);
                    Photo photo = new Photo()
                    {
                        Image = InputFileName,
                        User = user
                    };
                    _unitOfWork.Photos.Create(photo);
                }
            }
         }

        [Authorize]
       public ActionResult DeletePhoto(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo pic = _unitOfWork.Photos.Read(id);
            return View(pic);
        }
        
        
        [HttpPost, ActionName("DeletePhoto")]
        [Authorize]
        public ActionResult DeleteConfirmPhoto(Guid id)
        {
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;
            var photo = _unitOfWork.Photos.Read(id);
            var folder = Server.MapPath("~/Uploads/") + userName + "/" ;

            if (Directory.Exists(folder))
            {
                var filePath = folder + photo.Image;
                System.IO.File.Delete(filePath);
            }

            _unitOfWork.Photos.Delete(photo);
            _unitOfWork.Save();

            return RedirectToAction("Update","MyProfile"); 
        }


        [Authorize]
        public ActionResult DeleteEventPhoto(Guid photoId, Guid eventId)
        {
            try
            {
                var photo = _unitOfWork.Photos.Read(photoId);
                _unitOfWork.Events.DeletePhoto(photo, eventId);
                _unitOfWork.Save();
                return RedirectToAction("Update", "Event", new { eventId = eventId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            
        }
        //[HttpPost]
        //[Authorize, ActionName("DeleteEventPhoto")]
        //public ActionResult DeleteEventPhotoConfirm(Guid photoId,Guid eventId)
        //{
        //    var photo = _unitOfWork.Photos.Read(photoId);
        //    _unitOfWork.Events.DeletePhoto(photo,eventId);
        //    return RedirectToAction("Update", "Event", new { eventId = eventId  });   
        //}
    }
}


//$("#ticket, #day , #CategoryId").change(filtering).ready(filtering);

//function deletePhoto()
//{
//    $.ajax({
//    url: '/Photo/DeleteEventPhoto',
//        type: "POST",
//        cache: false,
//        dataType: 'html',
//        data: { photoId: $('#ticket').val(), photoId: $('#day').val()},
//        success: function(returnedEvents) {
//            $("#partial").html(returnedEvents);
//        },
//        error: function(xhr, textStatus, errorThrown) {
//            alert("failed");
//            console.log('STATUS: ' + textStatus + '\nERROR THROWN: ' + errorThrown);
//        }
//    })
//}