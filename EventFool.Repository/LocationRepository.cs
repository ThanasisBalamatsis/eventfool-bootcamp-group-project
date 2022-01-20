using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using System;
using EventFool.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EventFool.Repository
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(Eventfool dbcontext) : base(dbcontext) 
        { 

        }

        public Location Read(string locationName, string address)
        {
            return _dbContext.Locations.Where(l => l.Name == locationName && 
                                                   l.Address == address).FirstOrDefault();
        }

        public bool FindIfExistsInDB(string locationName, string address)
        {
            var result = _dbContext.Locations.Where(l => l.Name == locationName &&
                                                   l.Address == address).FirstOrDefault();

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IEnumerable<Location> Read (decimal swLat,decimal swLng,decimal neLat, decimal neLng)
        {
            return _dbContext.Locations.Where(l => l.Latitude >= swLat 
                                            && l.Latitude <= neLat 
                                            && l.Longitude <= neLng
                                            && l.Longitude >= swLng 
                                            && l.Events.Count!=0);
        }

        public IEnumerable<Location> ReadEagerEvents(decimal swLat, decimal swLng, decimal neLat, decimal neLng)
        {
            return _dbContext.Locations.Include("Events").Where(l => l.Latitude >= swLat
                                            && l.Latitude <= neLat
                                            && l.Longitude <= neLng
                                            && l.Longitude >= swLng
                                            && l.Events.Count != 0);
        }

        public Location GetEvents (Guid id)
        {
            return _dbContext.Locations.Include("Events").Where(l => l.Id == id).FirstOrDefault();
        }

        public IEnumerable<Event> AssociatedEvents(Guid id)
        {
            return _dbContext.Events.Where(i => i.Location.Id == id).ToList();
        }

        public override void Update(Location location)
        {
            _dbContext.Entry(location).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
