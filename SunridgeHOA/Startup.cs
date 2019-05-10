using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunridgeHOA.Data;
using SunridgeHOA.Models;

namespace SunridgeHOA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object UIFramework { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Set default password requirements
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller:exists}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //  name: "areas",
                //  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.EnsurePopulated(app, serviceProvider);
            CreateRoles(serviceProvider).Wait();
            //FixPrimaryOwners(serviceProvider).Wait();
            //CreateInitialIdentityUsers(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "SuperAdmin", "Admin", "Owner" };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (!userManager.Users.Any())
            {
                var owner = new Owner
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    Address = new Address
                    {
                        StreetAddress = "123 Testing",
                        City = "Testing",
                        State = "UT",
                        Zip = "84317"
                    }
                };
                context.Add(owner);
                context.SaveChanges();

                var superAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    //Email = "admin@email.com",
                    OwnerId = owner.OwnerId
                };

                var pass = "Password123$";

                var result = await userManager.CreateAsync(superAdmin, pass);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(superAdmin, new List<string> { "SuperAdmin", "Admin", "Owner" });
                }

                owner.ApplicationUserId = superAdmin.Id;
                context.SaveChanges();
            }
        }

        private async Task CreateInitialIdentityUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var owners = context.Owner;

            foreach (var owner in owners)
            {
                // The owner already has an identity user
                if (!String.IsNullOrEmpty(owner.ApplicationUserId))
                {
                    continue;
                }

                var username = await Areas.Admin.Data.OwnerUtility.GenerateUsername(userManager, owner);
                var defaultPassword = Areas.Admin.Data.OwnerUtility.GenerateDefaultPassword(owner);

                // Create user with default credentials
                //  - Username: FirstnameLastname (e.g. JessBrunker)
                //  - Password: 1234 (change in Areas/Admin/Data/OwnerUtility.cs)
                var newOwner = new ApplicationUser
                {
                    UserName = username,
                    Email = owner.Email,
                    OwnerId = owner.OwnerId
                };

                var result = await userManager.CreateAsync(newOwner, defaultPassword);
                if (result.Succeeded)
                {
                    var roles = new List<string> { "Owner" };

                    await userManager.AddToRolesAsync(newOwner, roles);

                    // Link Owner to the Application User
                    owner.ApplicationUserId = newOwner.Id;
                    //_context.Add(vm.Owner);
                    await context.SaveChangesAsync();
                }
            }
            
        }

        private async Task FixPrimaryOwners(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var lots = context.Lot.Include(u => u.OwnerLots);

            foreach (var lot in lots)
            {
                var primary = lot.OwnerLots.Where(u => u.IsPrimary);

                // If the lot has owners, and none of them are listed as a primary
                if (lot.OwnerLots.Any() && !primary.Any())
                {
                    // Only one owner on the lot, therefore they are primary
                    if (lot.OwnerLots.Count == 1)
                    {
                        lot.OwnerLots.ToList()[0].IsPrimary = true;
                    }
                    // More than one owner on the lot, just assume the first entry is primary (change later?)
                    else
                    {
                        lot.OwnerLots.ToList()[0].IsPrimary = true;
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}