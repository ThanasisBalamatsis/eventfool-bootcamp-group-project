using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFool.Domain;
using EventFool.Repository.Interfaces;


namespace EventFool.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Eventfool dbContext;
        public IEventRepository Events { get; }
        public IApplicationUserRepository Users { get; }
        public ITicketRepository Tickets { get; }
        public IPhotoRepository Photos { get; }
        public ILocationRepository Locations { get; }
        public ICategoryRepository Categories { get; }

        public UnitOfWork(Eventfool dbContext , IEventRepository Events, 
            IApplicationUserRepository Users,ILocationRepository Locations,
            ICategoryRepository Categories,IPhotoRepository Photos,ITicketRepository Ticket)
        {
            this.dbContext = dbContext;
            this.Events = Events;
            this.Users = Users;
            this.Locations = Locations;
            this.Categories = Categories;
            this.Photos = Photos;
            this.Tickets = Ticket;
        }
        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }
    }
}
