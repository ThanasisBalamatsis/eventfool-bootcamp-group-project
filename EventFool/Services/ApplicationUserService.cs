using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventFool.ViewModels;
using EventFool.Repository;

namespace EventFool.Services
{
    public static class ApplicationUserService
    {
        public static ApplicationUser MakeApplicationUserFromViewModel(UpdateUserViewModel updateUserViewModel)
        {
            
            

            ApplicationUser submittedUser = new ApplicationUser()
            {
                Id = updateUserViewModel.Id,
                FirstName = updateUserViewModel.FirstName,
                LastName = updateUserViewModel.LastName,
                Address = updateUserViewModel.Address,
                Birthdate = updateUserViewModel.Birthdate,
                Gender = updateUserViewModel.Gender,
                ProfilePhoto = updateUserViewModel.ProfilePhoto
            };
            
            return submittedUser;
        }
    }
}