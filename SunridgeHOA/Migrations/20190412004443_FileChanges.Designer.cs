﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SunridgeHOA.Models;

namespace SunridgeHOA.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190412004443_FileChanges")]
    partial class FileChanges
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apartment");

                    b.Property<string>("City");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<string>("State");

                    b.Property<string>("StreetAddress")
                        .IsRequired();

                    b.Property<string>("Zip");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("SunridgeHOA.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<int>("OwnerId");

                    b.Property<int?>("OwnerId1");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("OwnerId1");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Banner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<string>("Header");

                    b.Property<string>("Image");

                    b.HasKey("Id");

                    b.ToTable("Banner");
                });

            modelBuilder.Entity("SunridgeHOA.Models.ClassifiedCategory", b =>
                {
                    b.Property<int>("ClassifiedCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.HasKey("ClassifiedCategoryId");

                    b.ToTable("ClassifiedCategory");
                });

            modelBuilder.Entity("SunridgeHOA.Models.ClassifiedListing", b =>
                {
                    b.Property<int>("ClassifiedListingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassifiedCategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("ItemName");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<DateTime>("ListingDate");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Phone");

                    b.Property<float>("Price");

                    b.HasKey("ClassifiedListingId");

                    b.HasIndex("ClassifiedCategoryId");

                    b.HasIndex("OwnerId");

                    b.ToTable("ClassifiedListing");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("Date");

                    b.Property<int?>("LotHistoryId");

                    b.Property<int?>("OwnerHistoryId");

                    b.Property<int>("OwnerId");

                    b.Property<bool>("Private");

                    b.HasKey("CommentId");

                    b.HasIndex("LotHistoryId");

                    b.HasIndex("OwnerHistoryId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("SunridgeHOA.Models.CommonAreaAsset", b =>
                {
                    b.Property<int>("CommonAreaAssetId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssetName");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<float>("PurchasePrice");

                    b.Property<string>("Status");

                    b.HasKey("CommonAreaAssetId");

                    b.ToTable("CommonAreaAsset");
                });

            modelBuilder.Entity("SunridgeHOA.Models.ContactType", b =>
                {
                    b.Property<int>("ContactTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<string>("Value");

                    b.HasKey("ContactTypeId");

                    b.ToTable("ContactType");
                });

            modelBuilder.Entity("SunridgeHOA.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClassifiedListingId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("FileURL");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("LotHistoryId");

                    b.Property<string>("Type");

                    b.HasKey("FileId");

                    b.HasIndex("ClassifiedListingId");

                    b.HasIndex("LotHistoryId");

                    b.ToTable("File");
                });

            modelBuilder.Entity("SunridgeHOA.Models.HistoryType", b =>
                {
                    b.Property<int>("HistoryTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.HasKey("HistoryTypeId");

                    b.ToTable("HistoryType");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Inventory", b =>
                {
                    b.Property<int>("InventoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.HasKey("InventoryId");

                    b.ToTable("Inventory");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Key", b =>
                {
                    b.Property<int>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<string>("SerialNumber");

                    b.HasKey("KeyId");

                    b.ToTable("Key");
                });

            modelBuilder.Entity("SunridgeHOA.Models.KeyHistory", b =>
                {
                    b.Property<int>("KeyHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateIssued");

                    b.Property<DateTime>("DateReturned");

                    b.Property<bool>("IsArchive");

                    b.Property<int>("KeyId");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("OwnerId");

                    b.Property<float>("PaidAmount");

                    b.Property<string>("Status");

                    b.HasKey("KeyHistoryId");

                    b.HasIndex("KeyId");

                    b.HasIndex("OwnerId");

                    b.ToTable("KeyHistory");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Lot", b =>
                {
                    b.Property<int>("LotId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<string>("LotNumber")
                        .IsRequired();

                    b.Property<string>("TaxId");

                    b.HasKey("LotId");

                    b.HasIndex("AddressId");

                    b.ToTable("Lot");
                });

            modelBuilder.Entity("SunridgeHOA.Models.LotHistory", b =>
                {
                    b.Property<int>("LotHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("HistoryTypeId");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("LotId");

                    b.Property<string>("PrivacyLevel");

                    b.HasKey("LotHistoryId");

                    b.HasIndex("HistoryTypeId");

                    b.HasIndex("LotId");

                    b.ToTable("LotHistory");
                });

            modelBuilder.Entity("SunridgeHOA.Models.LotInventory", b =>
                {
                    b.Property<int>("LotInventoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("InventoryId");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("LotId");

                    b.HasKey("LotInventoryId");

                    b.HasIndex("InventoryId");

                    b.HasIndex("LotId");

                    b.ToTable("LotInventory");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Maintenance", b =>
                {
                    b.Property<int>("MaintenanceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommonAreaAssetId");

                    b.Property<float>("Cost");

                    b.Property<DateTime>("DateCompleted");

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.HasKey("MaintenanceId");

                    b.HasIndex("CommonAreaAssetId");

                    b.ToTable("Maintenance");
                });

            modelBuilder.Entity("SunridgeHOA.Models.NewsItem", b =>
                {
                    b.Property<int>("NewsItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("Header");

                    b.Property<string>("Image");

                    b.Property<int>("Year");

                    b.HasKey("NewsItemId");

                    b.ToTable("NewsItem");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Owner", b =>
                {
                    b.Property<int>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId");

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime?>("Birthday");

                    b.Property<string>("Email");

                    b.Property<string>("EmergencyContactName");

                    b.Property<string>("EmergencyContactPhone");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Occupation");

                    b.Property<string>("Phone");

                    b.HasKey("OwnerId");

                    b.HasIndex("AddressId");

                    b.ToTable("Owner");
                });

            modelBuilder.Entity("SunridgeHOA.Models.OwnerContactType", b =>
                {
                    b.Property<int>("OwnerContactTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContactTypeId");

                    b.Property<string>("ContactValue");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("OwnerId");

                    b.HasKey("OwnerContactTypeId");

                    b.HasIndex("ContactTypeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("OwnerContactType");
                });

            modelBuilder.Entity("SunridgeHOA.Models.OwnerHistory", b =>
                {
                    b.Property<int>("OwnerHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("HistoryTypeId");

                    b.Property<int?>("Hours");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Status");

                    b.HasKey("OwnerHistoryId");

                    b.HasIndex("HistoryTypeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("OwnerHistory");
                });

            modelBuilder.Entity("SunridgeHOA.Models.OwnerLot", b =>
                {
                    b.Property<int>("OwnerLotId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsArchive");

                    b.Property<bool>("IsPrimary");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("LotId");

                    b.Property<int>("OwnerId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("OwnerLotId");

                    b.HasIndex("LotId");

                    b.HasIndex("OwnerId");

                    b.ToTable("OwnerLot");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Photo", b =>
                {
                    b.Property<int>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<string>("Image");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Title");

                    b.Property<int>("Year");

                    b.HasKey("PhotoId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("SunridgeHOA.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DatePaid");

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<int>("LotId");

                    b.Property<int>("OwnerId");

                    b.Property<string>("Status");

                    b.Property<int>("TransactionTypeId");

                    b.HasKey("TransactionId");

                    b.HasIndex("LotId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("SunridgeHOA.Models.TransactionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchive");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedDate");

                    b.HasKey("Id");

                    b.ToTable("TransactionType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SunridgeHOA.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SunridgeHOA.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SunridgeHOA.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.ApplicationUser", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId1");
                });

            modelBuilder.Entity("SunridgeHOA.Models.ClassifiedListing", b =>
                {
                    b.HasOne("SunridgeHOA.Models.ClassifiedCategory", "ClassifiedCategory")
                        .WithMany()
                        .HasForeignKey("ClassifiedCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany("ClassifiedListings")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.Comment", b =>
                {
                    b.HasOne("SunridgeHOA.Models.LotHistory", "LotHistory")
                        .WithMany("Comments")
                        .HasForeignKey("LotHistoryId");

                    b.HasOne("SunridgeHOA.Models.OwnerHistory", "OwnerHistory")
                        .WithMany("Comments")
                        .HasForeignKey("OwnerHistoryId");

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.File", b =>
                {
                    b.HasOne("SunridgeHOA.Models.ClassifiedListing", "ClassifiedListing")
                        .WithMany()
                        .HasForeignKey("ClassifiedListingId");

                    b.HasOne("SunridgeHOA.Models.LotHistory", "LotHistory")
                        .WithMany("Files")
                        .HasForeignKey("LotHistoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.KeyHistory", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Key", "Key")
                        .WithMany()
                        .HasForeignKey("KeyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany("KeyHistories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.Lot", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.LotHistory", b =>
                {
                    b.HasOne("SunridgeHOA.Models.HistoryType", "HistoryType")
                        .WithMany("LotHistories")
                        .HasForeignKey("HistoryTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Lot", "Lot")
                        .WithMany("LotHistories")
                        .HasForeignKey("LotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.LotInventory", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Inventory", "Inventory")
                        .WithMany("LotInventories")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Lot", "Lot")
                        .WithMany("LotInventories")
                        .HasForeignKey("LotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.Maintenance", b =>
                {
                    b.HasOne("SunridgeHOA.Models.CommonAreaAsset", "CommonAreaAsset")
                        .WithMany("Maintenances")
                        .HasForeignKey("CommonAreaAssetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.Owner", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.OwnerContactType", b =>
                {
                    b.HasOne("SunridgeHOA.Models.ContactType", "ContactType")
                        .WithMany()
                        .HasForeignKey("ContactTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany("OwnerContactTypes")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.OwnerHistory", b =>
                {
                    b.HasOne("SunridgeHOA.Models.HistoryType", "HistoryType")
                        .WithMany()
                        .HasForeignKey("HistoryTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany("OwnerHistories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.OwnerLot", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Lot", "Lot")
                        .WithMany("OwnerLots")
                        .HasForeignKey("LotId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany("OwnerLots")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.Photo", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SunridgeHOA.Models.Transaction", b =>
                {
                    b.HasOne("SunridgeHOA.Models.Lot", "Lot")
                        .WithMany("Transactions")
                        .HasForeignKey("LotId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.Owner", "Owner")
                        .WithMany("Transactions")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SunridgeHOA.Models.TransactionType", "TransactionType")
                        .WithMany()
                        .HasForeignKey("TransactionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
