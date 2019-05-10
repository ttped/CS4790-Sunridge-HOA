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

            //Add default users and roles
            //if (!context.Owner.Any())
            //{
            //    context.Address.AddRange(
            //        new Address
            //        {
            //            //Id = 1,
            //            StreetAddress = "8146 Deer Run Way",
            //            City = "South Weber",
            //            State = "UT",
            //            Zip = "84405"
            //        },
            //        new Address
            //        {
            //            //Id = 2,
            //            StreetAddress = "11809 E Eagle Point Drive",
            //            City = "Huntsville",
            //            State = "UT",
            //            Zip = "84317"
            //        },
            //        new Address
            //        {
            //            //Id = 3,
            //            StreetAddress = "321 Willow Way",
            //            Apartment = "#69",
            //            City = "Pleasant View",
            //            State = "UT",
            //            Zip = "84414"
            //        },
            //        new Address
            //        {
            //            //Id = 4,
            //            StreetAddress = "11620 E Ptarmigan Point Drive",
            //            City = "Huntsville",
            //            State = "UT",
            //            Zip = "84317"
            //        },
            //        new Address
            //        {
            //            // Id = 5,
            //            StreetAddress = "11115 E Chukar Point Drive",
            //            City = "Huntsville",
            //            State = "UT",
            //            Zip = "84317"
            //        },
            //        new Address
            //        {
            //            //Id = 6,
            //            StreetAddress = "479 W Cheery Drive",
            //            City = "Riverdale",
            //            State = "UT",
            //            Zip = "84405"
            //        },
            //        new Address
            //        {
            //            //Id = 7,
            //            StreetAddress = "1016 E. Yale Avenue",
            //            City = "Salt Lake City",
            //            State = "UT",
            //            Zip = "84105"
            //        },
            //        new Address
            //        {
            //            //Id = 8,
            //            StreetAddress = "11426 E. Magpie Point Dr.",
            //            City = "Huntsville",
            //            State = "UT",
            //            Zip = "84317"
            //        },
            //        new Address
            //        {
            //            //Id = 9,
            //            StreetAddress = "11384 E. Magpie Point Dr.",
            //            City = "Huntsville",
            //            State = "UT",
            //            Zip = "84317"
            //        });

            //    context.SaveChanges();

            //    context.Owner.AddRange(
            //        new Owner
            //        {
            //            FirstName = "Super",
            //            LastName = "Admin"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 2,
            //            AddressId = 1,
            //            FirstName = "Richard",
            //            LastName = "Fry",
            //            Occupation = "Professor",
            //            Email = "admin@email.com"
            //        },
            //        new Owner
            //        {
            //            // OwnerId = 3,
            //            AddressId = 1,
            //            FirstName = "Stephen",
            //            LastName = "Merkley",
            //            Birthday = DateTime.ParseExact("10/10/1964", "MM/dd/yyyy", null),
            //            EmergencyContactName = "Richard Fry",
            //            EmergencyContactPhone = "8016266919",
            //            Email = "owner@email.com"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 4,
            //            AddressId = 3,
            //            FirstName = "Brent",
            //            LastName = "Frost",
            //            Occupation = "Retired",
            //            Email = "brentfrost@email.com"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 5,
            //            AddressId = 3,
            //            FirstName = "Vernie",
            //            LastName = "Frost",
            //            Occupation = "Retired",
            //            Email = "verniefrost@email.com"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 6,
            //            AddressId = 6,
            //            FirstName = "Andy",
            //            LastName = "Taylor",
            //            Occupation = "Contractor",
            //            EmergencyContactPhone = "8015102370",
            //            Email = "andytaylor@email.com"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 7,
            //            AddressId = 6,
            //            FirstName = "Stephanie",
            //            LastName = "Taylor",
            //            Occupation = "Credit Union Manager",
            //            EmergencyContactName = "Andy Taylor",
            //            Email = "stephanietaylor@email.com"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 8,
            //            AddressId = 7,
            //            FirstName = "Jennifer",
            //            LastName = "Hathorne",
            //            EmergencyContactPhone = "801-550-2247"
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 9,
            //            AddressId = 7,
            //            FirstName = "Jim",
            //            LastName = "Hathorne",
            //        },
            //        new Owner
            //        {
            //            //OwnerId = 10,
            //            AddressId = 7,
            //            FirstName = "Mike",
            //            LastName = "Green",
            //        });

            //    context.SaveChanges();
            //}

            //if (!context.Lot.Any())
            //{
            //    context.Lot.AddRange(
            //        new Lot
            //        {
            //            //LotId = 1,
            //            AddressId = 2,
            //            LotNumber = "H8",
            //            TaxId = "230660005"
            //        },
            //        new Lot
            //        {
            //            //LotId = 2,
            //            AddressId = 4,
            //            LotNumber = "H230",
            //            TaxId = "231180001"
            //        },
            //        new Lot
            //        {
            //            //LotId = 3,
            //            AddressId = 5,
            //            LotNumber = "H157",
            //            TaxId = "231000006"
            //        },
            //        new Lot
            //        {
            //            //LotId = 3,
            //            AddressId = 5,
            //            LotNumber = "H157",
            //            TaxId = "231000006"
            //        },
            //        new Lot
            //        {
            //            //LotId = 3,
            //            AddressId = 5,
            //            LotNumber = "H157",
            //            TaxId = "231000006"
            //        });

            //    context.SaveChanges();

            //    context.OwnerLot.AddRange(
            //        new OwnerLot
            //        {
            //            //OwnerLotId = 1,
            //            OwnerId = 2,
            //            LotId = 1,
            //            StartDate = DateTime.ParseExact("07/01/2018", "MM/dd/yyyy", null),
            //            IsPrimary = true
            //        },
            //        new OwnerLot
            //        {
            //           // OwnerLotId = 2,
            //            OwnerId = 3,
            //            LotId = 1,
            //            StartDate = DateTime.ParseExact("07/01/2018", "MM/dd/yyyy", null),
            //            IsPrimary = false
            //        },
            //        new OwnerLot
            //        {
            //            //OwnerLotId = 3,
            //            OwnerId = 4,
            //            LotId = 2,
            //            StartDate = DateTime.ParseExact("04/01/2002", "MM/dd/yyyy", null),
            //            IsPrimary = true
            //        },
            //        new OwnerLot
            //        {
            //            //OwnerLotId = 4,
            //            OwnerId = 5,
            //            LotId = 2,
            //            StartDate = DateTime.ParseExact("04/01/2002", "MM/dd/yyyy", null),
            //            IsPrimary = false
            //        },
            //        new OwnerLot
            //        {
            //            //OwnerLotId = 5,
            //            OwnerId = 6,
            //            LotId = 3,
            //            StartDate = DateTime.ParseExact("01/01/2019", "MM/dd/yyyy", null),
            //            IsPrimary = true
            //        },
            //        new OwnerLot
            //        {
            //            //OwnerLotId = 6,
            //            OwnerId = 7,
            //            LotId = 3,
            //            StartDate = DateTime.ParseExact("01/01/2019", "MM/dd/yyyy", null),
            //            IsPrimary = false
            //        });

            //    context.SaveChanges();
            //}

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
                    });
            }

            context.SaveChanges();
            if (!context.ClassifiedCategory.Any())
            {
                context.ClassifiedCategory.AddRange(
                    new ClassifiedCategory
                    {
                        Description = "Cabins"
                    },
                    new ClassifiedCategory
                    {
                        Description = "Lots"
                    },
                    new ClassifiedCategory
                    {
                        Description = "Other"
                    }
                    );
                context.SaveChanges();
            }

            //if (!context.LotInventory.Any())
            //{
            //    context.LotInventory.AddRange(
            //        new LotInventory
            //        {
            //            LotId = 1,
            //            InventoryId = 1,
            //            Description = "Septic tank"
            //        },
            //        new LotInventory
            //        {
            //            LotId = 1,
            //            InventoryId = 2,
            //            Description = "Solar panel"
            //        }
            //        );
            //    context.SaveChanges();
            //}

            //if (!context.Key.Any())
            //{
            //    context.Key.AddRange(
            //        new Key
            //        {
            //            SerialNumber = "28"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "29"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "30"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "31"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "1666"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "1662"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "1696"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "482"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "483"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "1487"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "485"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "988"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "989"
            //        },
            //        new Key
            //        {
            //            SerialNumber = "820501"
            //        }
            //        );
            //    context.SaveChanges();
            //}

            //if (!context.KeyHistory.Any())
            //{
            //    context.KeyHistory.AddRange(
            //        new KeyHistory
            //        {
            //            KeyId = 1,
            //            OwnerId = 2,
            //            Status = "Active",
            //            DateIssued = DateTime.Parse("1/1/2016"),
            //            DateReturned = DateTime.Parse("7/1/2018"),
            //            PaidAmount = 0
            //        },
            //        new KeyHistory
            //        {
            //            KeyId = 2,
            //            OwnerId = 3,
            //            Status = "Active",
            //            DateIssued = DateTime.Parse("1/1/2016"),
            //            DateReturned = DateTime.Parse("7/1/2018"),
            //            PaidAmount = 0
            //        },
            //        new KeyHistory
            //        {
            //            KeyId = 3,
            //            OwnerId = 2,
            //            Status = "Active",
            //            DateIssued = DateTime.Parse("1/1/2016"),
            //            DateReturned = DateTime.Parse("7/1/2018"),
            //            PaidAmount = 0
            //        },
            //        new KeyHistory
            //        {
            //            KeyId = 4,
            //            OwnerId = 2,
            //            Status = "Active",
            //            DateIssued = DateTime.Parse("1/1/2016"),
            //            DateReturned = DateTime.Parse("7/1/2018"),
            //            PaidAmount = 0
            //        },
            //        new KeyHistory
            //        {
            //            KeyId = 5,
            //            OwnerId = 2,
            //            Status = "Active",
            //            DateIssued = DateTime.Parse("1/1/2016"),
            //            DateReturned = DateTime.Parse("7/1/2018"),
            //            PaidAmount = 50
            //        }
            //        );
            //    context.SaveChanges();
            //}
        }
    }
}
