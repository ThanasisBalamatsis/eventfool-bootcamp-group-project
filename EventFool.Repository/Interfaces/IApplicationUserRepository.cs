using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFool.Domain.Models;

namespace EventFool.Repository.Interfaces
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        ApplicationUser Read(string id);
        ApplicationUser GetEventsTicketsPhotos(string userId);
    }
}
