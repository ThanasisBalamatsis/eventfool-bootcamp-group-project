using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Repository.Interfaces
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Location Read(string locationName, string address);

        bool FindIfExistsInDB(string locationName, string address);
        IEnumerable<Location> Read(decimal swLat, decimal swLng, decimal neLat, decimal neLng);
        IEnumerable<Location> ReadEagerEvents(decimal swLat, decimal swLng, decimal neLat, decimal neLng);
        Location GetEvents(Guid id);
        IEnumerable<Event> AssociatedEvents(Guid id);
    }
}
