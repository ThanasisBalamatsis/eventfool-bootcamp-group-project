using EventFool.Domain;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Repository
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(Eventfool dbContext) : base(dbContext)
        {

        }

        public ApplicationUser Read(string id)
        {
            return (_dbContext.Users.Find(id));
        }
        public override void Update(ApplicationUser submittedUser)
        {
            ApplicationUser existingUser = _dbContext.Users.Find(submittedUser.Id);

            existingUser.FirstName = submittedUser.FirstName;
            existingUser.LastName = submittedUser.LastName;
            existingUser.Address = submittedUser.Address;
            existingUser.Birthdate = submittedUser.Birthdate;
            existingUser.Gender = submittedUser.Gender;
            existingUser.ProfilePhoto = submittedUser.ProfilePhoto;
        }

        public ApplicationUser GetEventsTicketsPhotos(string userId)
        {
            return _dbContext.Users.Include(u => u.EventsOrganised).Include(u => u.Tickets).Include(u => u.Photos).Where(u => u.Id == userId).FirstOrDefault();
                                   
        }
    }
}
