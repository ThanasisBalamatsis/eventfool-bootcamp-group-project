using EventFool.Domain;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {

        public TicketRepository(Eventfool dbContext) : base(dbContext)
        {

        }

        public Ticket Read(Guid eventId, string userId)
        {
           return _dbContext.Tickets.Where(t => t.UserId == userId && t.EventId == eventId).FirstOrDefault();
        }

        public override void Update(Ticket ticket)
        {
            _dbContext.Entry(ticket).State = System.Data.Entity.EntityState.Modified;
        }

        public Ticket ReadLast(string userId)
        {
            return _dbContext.Tickets.Include("User").Include("Event").Where(x => x.UserId == userId).ToList().LastOrDefault();
        }
    }
}
