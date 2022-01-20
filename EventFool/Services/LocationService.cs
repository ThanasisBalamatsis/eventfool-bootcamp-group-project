using EventFool.Domain.Models;
using EventFool.ViewModels;
using EventFool.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.Services
{
    public static class LocationService
    {
        public static Location MakeLocationFromViewModel(UpdateEventViewModel updateEventViewModel, IUnitOfWork _unitOfWork)
        {
            Location previousLocation = _unitOfWork.Locations.Read(updateEventViewModel.LocationId);
            Location submittedLocation = new Location()
            {
                Name       = updateEventViewModel.LocationName,
                Address    = updateEventViewModel.Address,
                Latitude   = updateEventViewModel.Latitude,
                Longitude  = updateEventViewModel.Longitude
            };

            bool isSameLocation = CompareLocations(previousLocation, submittedLocation);

            if (isSameLocation)
            {
                return previousLocation;
            }
            else
            {
                bool isInDatabase = _unitOfWork.Locations.FindIfExistsInDB(submittedLocation.Name,
                                                                           submittedLocation.Address);
                if (isInDatabase)
                {
                    Location existingLocation = _unitOfWork.Locations.Read(submittedLocation.Name,
                                                                           submittedLocation.Address);
                    return existingLocation;
                }
                else
                {
                    _unitOfWork.Locations.Create(submittedLocation);
                    _unitOfWork.Save();
                    Location newLocation = _unitOfWork.Locations.Read(submittedLocation.Id);
                    return newLocation;
                }
                
            }

        }

        public static Location MakeLocationFromViewModel(EventCreateViewModel eventCreateViewModel, IUnitOfWork _unitOfWork)
        {
            Location submittedLocation = eventCreateViewModel.Location;

            bool isInDatabase = _unitOfWork.Locations.FindIfExistsInDB(submittedLocation.Name,
                                                                       submittedLocation.Address);
                                                                      

            if (isInDatabase)
            {
                Location existingLocation = _unitOfWork.Locations.Read(submittedLocation.Name,
                                                                       submittedLocation.Address);
                return existingLocation;
            }
            else
            {
                _unitOfWork.Locations.Create(submittedLocation);
                _unitOfWork.Save();
                Location newLocation = _unitOfWork.Locations.Read(submittedLocation.Id);
                return newLocation;
            }

        }

        public static bool CompareLocations(Location previousLocation, Location submittedLocation)
        {
            if (previousLocation.Name       != submittedLocation.Name || 
                previousLocation.Address    != submittedLocation.Address)
            {
                return false;
            }
            return true;
        }

        public static List<Location> CompareBounds(IUnitOfWork _unitOfWork, decimal swLat,decimal swLng,decimal neLat,decimal neLng)
        {
            var validLocations = _unitOfWork.Locations.ReadEagerEvents(swLat, swLng, neLat, neLng).ToList();

            return validLocations;
        }

        public static List<Event> LocationEvents(IUnitOfWork _unitOfWork, Guid id)
        {
            List<Event> EventsOfLocations = _unitOfWork.Locations.AssociatedEvents(id).ToList();

            return EventsOfLocations;
        }
    }
}