using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFool.Domain.Models;

namespace EventFool.Repository.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Ticket Read(Guid eventId, string userId);
        Ticket ReadLast(string userId);
    }
}
