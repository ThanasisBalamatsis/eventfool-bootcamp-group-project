using EventFool.Domain.Models;
using EventFool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.Services
{
    public static class ViewModelService
    {
        public static UpdateEventViewModel FillUpdateVM(Event theEvent)
        {
            UpdateEventViewModel updateEventViewModel = new UpdateEventViewModel()
            {
                Id = theEvent.Id,
                Name = theEvent.Name,
                Description = theEvent.Description,
                TicketPrice = theEvent.TicketPrice,
                MinTickets = theEvent.MinTickets,
                MaxTickets = theEvent.MaxTickets,

                LocationId = theEvent.Location.Id,
                LocationName = theEvent.Location.Name,
                Address = theEvent.Location.Address,
                City = theEvent.Location.City,
                State = theEvent.Location.State,
                PostalCode = theEvent.Location.PostalCode,
                Country = theEvent.Location.Country,
                Latitude = theEvent.Location.Latitude,
                Longitude = theEvent.Location.Longitude,

                CategoryId = theEvent.Category.Id,
                CategoryName = theEvent.Category.Name


            };
            return updateEventViewModel;
        }

    }
}