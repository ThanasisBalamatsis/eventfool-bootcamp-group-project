using EventFool.Repository.Interfaces;
using EventFool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using EventFool.Domain.Models;
using System.IO;
using System.Data;

namespace EventFool.Services
{
    public static class PhotoService
    {
        public static List<PhotoViewModel> GetPhotos(IUnitOfWork _unitOfWork , string userId,Guid eventId)
        {

            List<PhotoViewModel> eventCreateViewModel = new List<PhotoViewModel>();
            List<Photo> existingPhotos = _unitOfWork.Photos.ReadAll(userId).ToList();
            List<Photo> eventPhotos = _unitOfWork.Events.Read(eventId).Photos.ToList();
            existingPhotos = existingPhotos.Where(x => !eventPhotos.Contains(x)).ToList();
            //foreach (var photo in existingPhotos)
            //{

            //}

            foreach (var photo in existingPhotos)
            {
                //if (eventPhotos.Contains(photo))
                //{
                //    existingPhotos.Remove(photo);
                //}
                var photoViewModel = new PhotoViewModel();
                photoViewModel.Id = photo.Id;
                photoViewModel.Image = photo.Image;
                photoViewModel.Checked = false;
                eventCreateViewModel.Add(photoViewModel);
               
            }

            return eventCreateViewModel;

        }
        public static List<PhotoViewModel> GetPhotos(IUnitOfWork _unitOfWork, string userId)
        {

            List<PhotoViewModel> eventCreateViewModel = new List<PhotoViewModel>();
            List<Photo> existingPhotos = _unitOfWork.Photos.ReadAll(userId).ToList();

            foreach (var photo in existingPhotos)
            {
                var photoViewModel = new PhotoViewModel();
                photoViewModel.Id = photo.Id;
                photoViewModel.Image = photo.Image;
                photoViewModel.Checked = false;
                eventCreateViewModel.Add(photoViewModel);

            }

            return eventCreateViewModel;

        }

    }
}