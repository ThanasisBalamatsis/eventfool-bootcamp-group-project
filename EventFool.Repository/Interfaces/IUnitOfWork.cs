using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository Users { get; }
        IEventRepository Events { get; }
        ITicketRepository Tickets { get; }
        ILocationRepository Locations { get; }
        ICategoryRepository Categories { get; }
        IPhotoRepository Photos { get; }
        int Save();
    }
}
