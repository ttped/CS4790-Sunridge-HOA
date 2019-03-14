using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunridgeHOA.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityOwner>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Lot> Lot { get; set; }
        public DbSet<LotInventory> LotInventory { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<OwnerHistory> OwnerHistory { get; set; }
        public DbSet<HistoryType> HistoryType { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<ClassifiedListing> ClassifiedListing { get; set; }
        public DbSet<ClassifiedCategory> ClassifiedCategory { get; set; }
        public DbSet<KeyHistory> KeyHistory { get; set; }
        public DbSet<Key> Key { get; set; }
        public DbSet<OwnerContactType> OwnerContactType { get; set; }
        public DbSet<ContactType> ContactType { get; set; }
        public DbSet<CommonAreaAsset> CommonAreaAsset { get; set; }
        public DbSet<Maintenance> Maintenance { get; set; }

    }
}
