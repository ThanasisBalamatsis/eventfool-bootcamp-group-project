using EventFool.Domain;
using EventFool.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventFool.Startup))]
namespace EventFool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            Eventfool context = new Eventfool();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup I am creating first Admin Role and creating a default Admin User   
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                 

                var user = new ApplicationUser()
                {

                    UserName = "admin@admin.gr",
                    Email = "admin@admin.gr",
                    Address = "super-admin",
                    Gender = "Male",
                    FirstName = "Admin",
                    LastName = "Super",
                    Birthdate= new System.DateTime(1990,1,1)
                   
                };

                string userPWD = "Admin123!";

                var chkUser = UserManager.Create(user, userPWD);
                //context.SaveChanges();

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

  
        }
    }
}
