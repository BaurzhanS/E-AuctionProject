using E_Auction.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Infrastructure
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionFileMeta> AuctionFileMeta { get; set; }
        public DbSet<AuctionCategory> AuctionCategories { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AuctionWinner> AuctionWinners { get; set; }
        public DbSet<OrganizationRating> OrganizationRatings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Organization>()
                .HasMany(p => p.Auctions)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId);

            modelBuilder
                .Entity<Organization>()
                .HasMany(p => p.AuctionWinners)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            modelBuilder
                .Entity<Organization>()
                .HasMany(p => p.Bids)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            modelBuilder
                .Entity<Organization>()
                .HasMany(p => p.OrganizationRatings)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId);

            modelBuilder
                .Entity<Auction>()
                .HasRequired(p => p.Category)
                .WithMany(p => p.Auctions)
                .HasForeignKey(p => p.AuctionCategoryId);

            modelBuilder
                .Entity<Auction>()
                .HasMany(p => p.Files)
                .WithRequired(p => p.Auction)
                .HasForeignKey(p => p.AuctionId);

            modelBuilder
                .Entity<Auction>()
                .HasMany(p => p.Bids)
                .WithRequired(p => p.Auction)
                .HasForeignKey(p => p.AuctionId);

            modelBuilder.Entity<Auction>()
                .HasOptional(p => p.AuctionWinner)
                .WithRequired(p => p.Auction);

            modelBuilder
               .Entity<Auction>()
               .HasMany(p => p.OrganizationRatings)
               .WithRequired(p => p.Auction)
               .HasForeignKey(p => p.AuctionId).WillCascadeOnDelete(false);

            modelBuilder
                .Entity<EmployeePosition>()
                .HasMany(p => p.Employees)
                .WithRequired(p => p.EmployeePosition)
                .HasForeignKey(p => p.EmployeePositionId);

            modelBuilder.Entity<Organization>()
                .HasMany(p => p.Transactions)
                .WithRequired(p => p.Organization)
                .HasForeignKey(p => p.OrganizationId);
            base.OnModelCreating(modelBuilder);
        }

        public AplicationDbContext() : base("E-AuctionConnectionString")
        {

        }
    }
}
