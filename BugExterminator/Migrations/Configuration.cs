namespace BugExterminator.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                role = new IdentityRole { Name = "Project Manager" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                role = new IdentityRole { Name = "Developer" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                role = new IdentityRole { Name = "Submitter" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Tester"))
            {
                role = new IdentityRole { Name = "Tester" };
                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (!context.Users.Any(u => u.Email == "jddzul86@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jddzul86@gmail.com",
                    Email = "jddzul86@gmail.com",
                    FirstName = "Daniel",
                    LastName = "Dzul",
                    DisplayName = "Daniel Dzul"
                },
                "Jddzul&789521");
            }

            var userId = userManager.FindByEmail("jddzul86@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "DemoAdmin@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@gmail.com",
                    Email = "DemoAdmin@gmail.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                    DisplayName = "Demo Admin"
                },
                "Abc&123");
            }
            var DemoAdmin = userManager.FindByEmail("DemoAdmin@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "DemoDeveloper@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@gmail.com",
                    Email = "DemoDeveloper@gmail.com",
                    FirstName = "Demo",
                    LastName = "Developer",
                    DisplayName = "Demo Developer"
                },
                "Abc&123");
            }

            userId = userManager.FindByEmail("DemoDeveloper@gmail.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@gmail.com",
                    Email = "DemoSubmitter@gmail.com",
                    FirstName = "Demo",
                    LastName = "Submitter",
                    DisplayName = "Demo Submitter"
                },
                "Abc&123");
            }

            userId = userManager.FindByEmail("DemoDeveloper@gmail.com").Id;
            userManager.AddToRole(userId, "Submitter");


            if (!context.Users.Any(u => u.Email == "UserOne@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "UserOne@gmail.com",
                    Email = "UserOne@gmail.com",
                    FirstName = "John",
                    LastName = "Doe",
                    DisplayName = "John Doe"
                },
                    "Abc&123");
            }

            if (!context.Users.Any(u => u.Email == "UserTwo@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "UserTwo@gmail.com",
                    Email = "UserTwo@gmail.com",
                    FirstName = "Carlos",
                    LastName = "Torres",
                    DisplayName = "Carlos Torres"
                },
                    "Abc&123");
            }

            if (!context.Users.Any(u => u.Email == "UserThree@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "UserThree@gmail.com",
                    Email = "UserThree@gmail.com",
                    FirstName = "Jose",
                    LastName = "Mingo",
                    DisplayName = "Jose Mingo"
                },
                    "Abc&123");
            }

            if (!context.Users.Any(u => u.Email == "UserFour@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "UserFour@gmail.com",
                    Email = "UserFour@gmail.com",
                    FirstName = "Tom",
                    LastName = "Smith",
                    DisplayName = "Tom Smith"
                },
                    "Abc&123");
            }
            if (!context.TicketPriority.Any(u => u.Name == "High"))
            {
                context.TicketPriority.Add(new TicketPriority { Name = "High" });
            }

            if (!context.TicketPriority.Any(u => u.Name == "Medium"))
            {
                context.TicketPriority.Add(new TicketPriority { Name = "Medium" });
            }

            if (!context.TicketPriority.Any(u => u.Name == "Low"))
            {
                context.TicketPriority.Add(new TicketPriority { Name = "Low" });
            }

            if (!context.TicketPriority.Any(u => u.Name == "Urgent"))
            {
                context.TicketPriority.Add(new TicketPriority { Name = "Urgent" });
            }

            if (!context.TicketType.Any(u => u.Name == "Fix"))
            {
                context.TicketType.Add(new TicketType { Name = "Fix" });
            }

            if (!context.TicketType.Any(u => u.Name == "Update"))
            {
                context.TicketType.Add(new TicketType { Name = "Update" });
            }

            if (!context.TicketType.Any(u => u.Name == "Task"))
            {
                context.TicketType.Add(new TicketType { Name = "Task" });
            }

            if (!context.TicketStatus.Any(u => u.Name == "New"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "New" });
            }

            if (!context.TicketStatus.Any(u => u.Name == "In Development"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "In Development" });
            }

            if (!context.TicketStatus.Any(u => u.Name == "Complete"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "Complete" });
            }
        }
    }
}
