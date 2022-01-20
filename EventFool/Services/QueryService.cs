using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.Services
{
    public  static class QueryService
    {
        public static List<FilteredEventsViewModel> FindFilteredEvents(IEnumerable<Event> allEvents,IUnitOfWork _unitOfWork)
        {
            List<FilteredEventsViewModel> filteredEvents = new List<FilteredEventsViewModel>();
            foreach (Event myEvent in allEvents)
            {
                FilteredEventsViewModel filteredEventsViewModel = new FilteredEventsViewModel()
                {
                    Id = myEvent.Id,
                    LocationId = myEvent.Location.Id,
                    LocationName = myEvent.Location.Name,
                    Name = myEvent.Name,
                    StartDate = myEvent.StartDate,
                    TicketPrice = myEvent.TicketPrice,
                    ProfilePhoto = myEvent.ProfilePhoto,
                    UserName = _unitOfWork.Events.ReadEager(myEvent.Id).Users.FirstOrDefault().UserName
                };
                filteredEvents.Add(filteredEventsViewModel);
            }
            return filteredEvents;
        }

        #region Created Events Filters
        public static List<Event> FilterCreatedBasedOnPrice(IUnitOfWork _unitOfWork, string price, string userId)
        {
            if (price == "Ticket")
            {
                List<Event> allEvents = _unitOfWork.Users.Read(userId).EventsOrganised.ToList(); 
                return allEvents;
            }
            List<Event> filteredEvents = _unitOfWork.Users.Read(userId).EventsOrganised.Where(e => e.TicketPrice <= float.Parse(price)).ToList();

            return filteredEvents;
        }

        public static List<Event> FilterCreatedBasedOnDate(string day, IEnumerable<Event> allEvents)
        {
            if (day == "Day")
            {
                return allEvents.ToList();
            }
            List<Event> filteredEvents = allEvents.Where(e => e.EndDate >= System.DateTime.Now &&
                                                                     e.EndDate >= System.DateTime.Now.AddDays(double.Parse(day))).ToList();

            return filteredEvents;
        }

        public static List<Event> FilterCreatedBasedOnDateSearch(string day, IEnumerable<Event> allEvents)
        {
            if (day == "Day")
            {
                List<Event> results = allEvents.Where(e => e.EndDate >= System.DateTime.Now).ToList();

                return results;
            }
            List<Event> filteredEvents = allEvents.Where(e => e.EndDate >= System.DateTime.Now && System.DateTime.Now.AddDays(double.Parse(day)) <= e.EndDate).ToList();
            return filteredEvents;
        }

        public static List<Event> FilterCreatedBasedOnCategory(string categoryId, IEnumerable<Event> allEvents)
        {
            if (categoryId == null)
            {
                return allEvents.ToList();
            }
            List<Event> filteredEvents = allEvents.Where(e => e.Category.Id == Guid.Parse(categoryId)).ToList();
            return filteredEvents;
        }


        #endregion



        #region Attended Events Filters
        public static List<Event> FilterAttendedBasedOnPrice(IUnitOfWork _unitOfWork, string price, string userId)
        {
            if (price == "Ticket")
            {
                List<Event> allEvents = _unitOfWork.Users.Read(userId)
                                                         .Tickets.Where(t => t.Active == true)
                                                         .Select(t => t.Event).ToList();
                return allEvents;
            }
            List<Event> filteredEvents = _unitOfWork.Users.Read(userId)
                                                          .Tickets.Where(t => t.Price <= float.Parse(price))
                                                          .Select(t => t.Event).ToList();
            return filteredEvents; 
        }

        public static List<Event> FilterAttendedBasedOnDate(string day, IEnumerable<Event> allEvents)
        {
            if (day == "Day")
            {
                return allEvents.ToList(); 
            }
            List<Event> filteredEvents = allEvents.Where(e => e.EndDate >= System.DateTime.Now && 
                                                                     e.EndDate >= System.DateTime.Now.AddDays(double.Parse(day))).ToList();

            return filteredEvents;
        }

        

        public static List<Event> FilterAttendedBasedOnCategory (string categoryId , IEnumerable<Event> allEvents)
        {
            if (categoryId == null)
            {
                return allEvents.ToList();
            }
            List<Event> filteredEvents = allEvents.Where(e => e.Category.Id == Guid.Parse(categoryId)).ToList();
            return filteredEvents;
        }
        #endregion

        
        #region All Events Filters
        // public static List<Event> FilterAllEventsBasedOnPrice(IUnitOfWork _unitOfWork, string price)
        // {
        //     List<Event> filteredEvents = new List<Event>();
        //     if (price == "Ticket")
        //     { 
        //         locations.ForEach( x => x.Events.Where(ev => ev.EndDate >= DateTime.Now).ToList().ForEach(e => filteredEvents.Add(e)));
        //         return filteredEvents;
        //     }
        //     //List<Event> filteredEvents = _unitOfWork.Events.ReadAll().Where(e => e.EndDate >= System.DateTime.Now 
        //     //                                                                  && e.TicketPrice <= float.Parse(price)).ToList();

        //     locations.ForEach(x => x.Events.Where(e => e.EndDate >= System.DateTime.Now && e.TicketPrice <= float.Parse(price)).ToList().ForEach(e => filteredEvents.Add(e)));
        //     return filteredEvents;
        // }

        public static List<Event> FilterAllEventsBasedOnDate(string day, IEnumerable<Event> allEvents)
        {
            if (day == "Day")
            {
                return allEvents.ToList();
            }
            List<Event> filteredEvents = allEvents.Where(e => e.EndDate >= System.DateTime.Now &&
                                                              e.EndDate >= System.DateTime.Now.AddDays(double.Parse(day))).ToList();

            return filteredEvents;
        }

        public static List<Event> FilterAllEventsBasedOnCategory(string categoryId, IEnumerable<Event> allEvents)
        {
            if (categoryId == null)
            {
                return allEvents.ToList();
            }
            List<Event> filteredEvents = allEvents.Where(e => e.Category.Id == Guid.Parse(categoryId)).ToList();
            return filteredEvents;
        }
        #endregion



        public static bool FilterEventBasedOnDropdowns(Event myEvent, string day, string price, string categoryId)
        {

            if ((day == "Day" && myEvent.EndDate >= System.DateTime.Now) || 
                (myEvent.EndDate >= System.DateTime.Now && myEvent.EndDate <= System.DateTime.Now.AddHours(double.Parse(day)-DateTime.Now.Hour)))
            {
                if ((price == "Ticket") || (myEvent.TicketPrice <= float.Parse(price)))
                {
                    if ((categoryId == null) || (myEvent.Category.Id == Guid.Parse(categoryId)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


    

    }
}