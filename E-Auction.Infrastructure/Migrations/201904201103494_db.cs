namespace E_Auction.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuctionCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ShippingAddress = c.String(),
                        ShippingConditions = c.String(),
                        PriceAtStart = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceChangeStep = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceAtMinimum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        FinishDateExpected = c.DateTime(nullable: false),
                        FinishDateActual = c.DateTime(),
                        AuctionStatus = c.Int(nullable: false),
                        AuctionCategoryId = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuctionCategories", t => t.AuctionCategoryId, cascadeDelete: true)
                .Index(t => t.AuctionCategoryId);
            
            CreateTable(
                "dbo.AuctionWinners",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        AuctionId = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .ForeignKey("dbo.Auctions", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        IdentificationNumber = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        OrganizationTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrganizationTypes", t => t.OrganizationTypeId, cascadeDelete: true)
                .Index(t => t.OrganizationTypeId);
            
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        BidStatus = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        AuctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.AuctionId);
            
            CreateTable(
                "dbo.OrganizationRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Point = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                        AuctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Auctions", t => t.AuctionId)
                .Index(t => t.OrganizationId)
                .Index(t => t.AuctionId);
            
            CreateTable(
                "dbo.OrganizationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionType = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.AuctionFileMetas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Extension = c.String(),
                        ContentAsBase64 = c.Binary(),
                        CreatedAt = c.DateTime(nullable: false),
                        AuctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .Index(t => t.AuctionId);
            
            CreateTable(
                "dbo.EmployeePositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DoB = c.DateTime(nullable: false),
                        Email = c.String(),
                        EmployeePositionId = c.Int(nullable: false),
                        OrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeePositions", t => t.EmployeePositionId, cascadeDelete: true)
                .Index(t => t.EmployeePositionId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.OrganizationsAuctions",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false),
                        AuctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrganizationId, t.AuctionId })
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.AuctionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "EmployeePositionId", "dbo.EmployeePositions");
            DropForeignKey("dbo.Employees", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.OrganizationRatings", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.AuctionFileMetas", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "AuctionCategoryId", "dbo.AuctionCategories");
            DropForeignKey("dbo.Bids", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.AuctionWinners", "Id", "dbo.Auctions");
            DropForeignKey("dbo.Transactions", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "OrganizationTypeId", "dbo.OrganizationTypes");
            DropForeignKey("dbo.OrganizationRatings", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Bids", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.AuctionWinners", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.OrganizationsAuctions", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.OrganizationsAuctions", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.OrganizationsAuctions", new[] { "AuctionId" });
            DropIndex("dbo.OrganizationsAuctions", new[] { "OrganizationId" });
            DropIndex("dbo.Employees", new[] { "OrganizationId" });
            DropIndex("dbo.Employees", new[] { "EmployeePositionId" });
            DropIndex("dbo.AuctionFileMetas", new[] { "AuctionId" });
            DropIndex("dbo.Transactions", new[] { "OrganizationId" });
            DropIndex("dbo.OrganizationRatings", new[] { "AuctionId" });
            DropIndex("dbo.OrganizationRatings", new[] { "OrganizationId" });
            DropIndex("dbo.Bids", new[] { "AuctionId" });
            DropIndex("dbo.Bids", new[] { "OrganizationId" });
            DropIndex("dbo.Organizations", new[] { "OrganizationTypeId" });
            DropIndex("dbo.AuctionWinners", new[] { "OrganizationId" });
            DropIndex("dbo.AuctionWinners", new[] { "Id" });
            DropIndex("dbo.Auctions", new[] { "AuctionCategoryId" });
            DropTable("dbo.OrganizationsAuctions");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeePositions");
            DropTable("dbo.AuctionFileMetas");
            DropTable("dbo.Transactions");
            DropTable("dbo.OrganizationTypes");
            DropTable("dbo.OrganizationRatings");
            DropTable("dbo.Bids");
            DropTable("dbo.Organizations");
            DropTable("dbo.AuctionWinners");
            DropTable("dbo.Auctions");
            DropTable("dbo.AuctionCategories");
        }
    }
}
