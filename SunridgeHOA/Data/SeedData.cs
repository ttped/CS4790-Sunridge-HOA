using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SunridgeHOA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            //context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Add default users and roles
            if (!context.Owner.Any())
            {
                context.Address.AddRange(
                    new Address
                    {
                        //Id = 1,
                        StreetAddress = "8146 Deer Run Way",
                        City = "South Weber",
                        State = "UT",
                        Zip = "84405"
                    },
                    new Address
                    {
                        //Id = 2,
                        StreetAddress = "11809 E Eagle Point Drive",
                        City = "Huntsville",
                        State = "UT",
                        Zip = "84317"
                    },
                    new Address
                    {
                        //Id = 3,
                        StreetAddress = "321 Willow Way",
                        Apartment = "#69",
                        City = "Pleasant View",
                        State = "UT",
                        Zip = "84414"
                    },
                    new Address
                    {
                        //Id = 4,
                        StreetAddress = "11620 E Ptarmigan Point Drive",
                        City = "Huntsville",
                        State = "UT",
                        Zip = "84317"
                    },
                    new Address
                    {
                       // Id = 5,
                        StreetAddress = "11115 E Chukar Point Drive",
                        City = "Huntsville",
                        State = "UT",
                        Zip = "84317"
                    },
                    new Address
                    {
                        //Id = 6,
                        StreetAddress = "479 W Cheery Drive",
                        City = "Riverdale",
                        State = "UT",
                        Zip = "84405"
                    });

                context.SaveChanges();

                context.Owner.AddRange(
                    new Owner
                    {
                        //OwnerId = 1,
                        AddressId = 1,
                        FirstName = "Richard",
                        LastName = "Fry",
                        Occupation = "Professor",
                        Email = "admin@email.com"
                    },
                    new Owner
                    {
                       // OwnerId = 2,
                        AddressId = 1,
                        FirstName = "Stephen",
                        LastName = "Merkley",
                        Birthday = DateTime.ParseExact("10/10/1964", "MM/dd/yyyy", null),
                        EmergencyContactName = "Richard Fry",
                        EmergencyContactPhone = "8016266919",
                        Email = "owner@email.com"
                    },
                    new Owner
                    {
                        //OwnerId = 3,
                        AddressId = 3,
                        FirstName = "Brent",
                        LastName = "Frost",
                        Occupation = "Retired",
                        Email = "brentfrost@email.com"
                    },
                    new Owner
                    {
                        //OwnerId = 4,
                        AddressId = 3,
                        FirstName = "Vernie",
                        LastName = "Frost",
                        Occupation = "Retired",
                        Email = "verniefrost@email.com"
                    },
                    new Owner
                    {
                        //OwnerId = 5,
                        AddressId = 6,
                        FirstName = "Andy",
                        LastName = "Taylor",
                        Occupation = "Contractor",
                        EmergencyContactPhone = "8015102370",
                        Email = "andytaylor@email.com"
                    },
                    new Owner
                    {
                        //OwnerId = 6,
                        AddressId = 6,
                        FirstName = "Stephanie",
                        LastName = "Taylor",
                        Occupation = "Credit Union Manager",
                        EmergencyContactName = "Andy Taylor",
                        Email = "stephanietaylor@email.com"
                    });

                context.SaveChanges();
            }

            if (!context.Lot.Any())
            {
                context.Lot.AddRange(
                    new Lot
                    {
                        //LotId = 1,
                        AddressId = 2,
                        LotNumber = "H8",
                        TaxId = "230660005"
                    },
                    new Lot
                    {
                        //LotId = 2,
                        AddressId = 4,
                        LotNumber = "H230",
                        TaxId = "231180001"
                    },
                    new Lot
                    {
                        //LotId = 3,
                        AddressId = 5,
                        LotNumber = "H157",
                        TaxId = "231000006"
                    });

                context.SaveChanges();

                context.OwnerLot.AddRange(
                    new OwnerLot
                    {
                        //OwnerLotId = 1,
                        OwnerId = 1,
                        LotId = 1,
                        StartDate = DateTime.ParseExact("07/01/2018", "MM/dd/yyyy", null),
                        IsPrimary = true
                    },
                    new OwnerLot
                    {
                       // OwnerLotId = 2,
                        OwnerId = 2,
                        LotId = 1,
                        StartDate = DateTime.ParseExact("07/01/2018", "MM/dd/yyyy", null),
                        IsPrimary = false
                    },
                    new OwnerLot
                    {
                        //OwnerLotId = 3,
                        OwnerId = 3,
                        LotId = 2,
                        StartDate = DateTime.ParseExact("04/01/2002", "MM/dd/yyyy", null),
                        IsPrimary = true
                    },
                    new OwnerLot
                    {
                        //OwnerLotId = 4,
                        OwnerId = 4,
                        LotId = 2,
                        StartDate = DateTime.ParseExact("04/01/2002", "MM/dd/yyyy", null),
                        IsPrimary = false
                    },
                    new OwnerLot
                    {
                        //OwnerLotId = 5,
                        OwnerId = 5,
                        LotId = 3,
                        StartDate = DateTime.ParseExact("01/01/2019", "MM/dd/yyyy", null),
                        IsPrimary = true
                    },
                    new OwnerLot
                    {
                        //OwnerLotId = 6,
                        OwnerId = 6,
                        LotId = 3,
                        StartDate = DateTime.ParseExact("01/01/2019", "MM/dd/yyyy", null),
                        IsPrimary = false
                    });

                context.SaveChanges();
            }

            if (!context.Inventory.Any())
            {
                // Add temporary inventory items
                context.Inventory.AddRange(
                    new Inventory
                    {
                        Description = "Septic tank",
                        LastModifiedBy = "Seed",
                        LastModifiedDate = DateTime.Now
                    },
                    new Inventory
                    {
                        Description = "Solar panel",
                        LastModifiedBy = "Seed",
                        LastModifiedDate = DateTime.Now
                    },
                    new Inventory
                    {
                        Description = "Outhouse",
                        LastModifiedBy = "Seed",
                        LastModifiedDate = DateTime.Now
                    });
            }

// Sean
            context.SaveChanges();
//---------
            if (!context.ClassifiedCategory.Any())
            {
                context.ClassifiedCategory.AddRange(
                    new ClassifiedCategory
                    {
                    },
                    new ClassifiedCategory
                    {
                    }
                    );
                context.SaveChanges();
            }
//>>>>>>> master
        }
    }
}
