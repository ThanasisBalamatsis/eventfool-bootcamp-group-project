using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFool.Domain.Models;

namespace EventFool.Repository.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Event GetLocationCategory(Guid id);

        void Create(Event entity, string userId);
        void Create(Guid entity, Photo photo);

        List<Event> ReadAllEager();

        Event ReadEager(Guid eventid);

        void DeletePhoto(Photo photo, Guid eventId);
        List<Event> FindTrendingEvents();

    }
}
