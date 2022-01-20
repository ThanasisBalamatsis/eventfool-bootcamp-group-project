using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.Services
{
    public static class EventService
    {
        public static Event MakeEventFromViewModel(UpdateEventViewModel updateEventViewModel, IUnitOfWork _unitOfWork)
        {
            Event myEvent = new Event()
            {
                Id = updateEventViewModel.Id,
                Name = updateEventViewModel.Name,
                Description = updateEventViewModel.Description,
                TicketPrice = updateEventViewModel.TicketPrice,
                MinTickets = updateEventViewModel.MinTickets,
                MaxTickets = updateEventViewModel.MaxTickets,
                Location = LocationService.MakeLocationFromViewModel(updateEventViewModel, _unitOfWork),
                Category = _unitOfWork.Categories.Read(updateEventViewModel.CategoryId),
                StartDate = updateEventViewModel.StartDate,
                EndDate = updateEventViewModel.EndDate,
                ProfilePhoto = updateEventViewModel.ProfilePhoto

            };
            return myEvent;
        }

        public static Event MakeEventFromViewModel(EventCreateViewModel eventCreateViewModel, IUnitOfWork _unitOfWork)
        {

            var myEvent = new Event()
            {
                Name = eventCreateViewModel.Name,
                Description = eventCreateViewModel.Description,
                TicketPrice = eventCreateViewModel.TicketPrice,
                MinTickets = eventCreateViewModel.MinTickets,
                MaxTickets = eventCreateViewModel.MaxTickets,
                Location = LocationService.MakeLocationFromViewModel(eventCreateViewModel, _unitOfWork),
                Category = _unitOfWork.Categories.Read(eventCreateViewModel.CategoryId),
                StartDate = eventCreateViewModel.StartDate,
                EndDate = eventCreateViewModel.EndDate,
                ProfilePhoto = eventCreateViewModel.ProfilePhoto
            };
            return myEvent;
        }


        public static EventProfileViewModel MakeViewModelFromEvent(Event myEvent, IUnitOfWork _unitOfWork)
        {
            myEvent = _unitOfWork.Events.ReadEager(myEvent.Id);
            List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            _unitOfWork.Events.Read(myEvent.Id).Tickets.ToList().ForEach(x => applicationUsers.Add(_unitOfWork.Users.Read(x.UserId)));
            var eventProfileViewModel = new EventProfileViewModel()
            {
                EventId = myEvent.Id,
                UserName = _unitOfWork.Events.ReadEager(myEvent.Id).Users.FirstOrDefault().UserName,
                Name = myEvent.Name,
                RemainingTickets = myEvent.MaxTickets - myEvent.Tickets.Count,
                Description = myEvent.Description,
                StartDate = myEvent.StartDate,
                EndDate = myEvent.EndDate,
                Location = myEvent.Location,
                ProfilePhoto = myEvent.ProfilePhoto,
                Attendees = applicationUsers,
                Photos = myEvent.Photos.ToList(),
                Category = myEvent.Category,
                TicketPrice = myEvent.TicketPrice,
                Tickets = myEvent.Tickets.ToList(),

            };
            return eventProfileViewModel;

        }
    }
}