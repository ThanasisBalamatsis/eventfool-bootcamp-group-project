using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using EventFool.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace EventFool.Models
{

    //public class MyRole : IdentityRole<Guid, MyUserRole>
    //{
    //    public MyRole() { Id = Guid.NewGuid(); }
    //    public MyRole(string name) : this() { Name = name; }
    //}
    //public class MyUserRole : IdentityUserRole<Guid> { }
    //public class MyUserClaim : IdentityUserClaim<Guid> { }
    //public class MyUserLogin : IdentityUserLogin<Guid> { }


    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("EventfoolDB", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}
}