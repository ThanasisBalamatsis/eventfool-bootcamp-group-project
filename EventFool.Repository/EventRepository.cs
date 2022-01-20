using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using EventFool.Domain;
using System.Web;

namespace EventFool.Repository
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(Eventfool dbContext) : base(dbContext)
        {
            
        }

        public Event GetLocationCategory(Guid eventId)
        {
            return _dbContext.Events.Include("Location").Include("Category").Where(t => t.Id == eventId).FirstOrDefault();
            
        }

       

        public override void Update(Event entity)
        {
            var myEvent = _dbContext.Events.Include("Location").Include("Category").Where(e => e.Id == entity.Id).FirstOrDefault();

            myEvent.Category = entity.Category;
            myEvent.Location = entity.Location;
            myEvent.Name = entity.Name;
            myEvent.Description = entity.Description;
            myEvent.StartDate = entity.StartDate;
            myEvent.EndDate = entity.EndDate;
            myEvent.MinTickets = entity.MinTickets;
            myEvent.MaxTickets = entity.MaxTickets;
            myEvent.TicketPrice = entity.TicketPrice;
            myEvent.ProfilePhoto = entity.ProfilePhoto;

        }

        public void Create(Event entity, string userId)
        {
            _dbContext.Users.Find(userId).EventsOrganised.Add(entity);
        }
        public void Create(Guid entity,Photo photo)
        {
            _dbContext.Events.Find(entity).Photos.Add(photo);
        }

        public List<Event> ReadAllEager()
        {
            return _dbContext.Events.Include("Location").Include("Category").ToList();
        }


        public Event ReadEager(Guid eventid)
        {


            return _dbContext.Events.Include("Location").Include("Category").Include("Tickets").Include("Photos").Include("Users").FirstOrDefault(x => x.Id == eventid);
        }

        public void DeletePhoto(Photo photo,Guid eventId)
        {
            var myEvent = _dbContext.Events.Find(eventId);
            _dbContext.Events.Find(eventId).Photos.Remove(photo);
            _dbContext.Photos.Find(photo.Id).Events.Remove(myEvent);
        }

        public List<Event> FindTrendingEvents()
        {
            List<Event> myEvents = _dbContext.Events.Include("Location").Include("Tickets").Include("Users").OrderByDescending(e => e.Tickets.Count()).ToList();
            return myEvents;
        }

      
    }
}
