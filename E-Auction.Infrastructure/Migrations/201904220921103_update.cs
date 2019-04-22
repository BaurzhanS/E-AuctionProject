namespace E_Auction.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "Address", c => c.String());
            AddColumn("dbo.Organizations", "PhoneNumber", c => c.String());
            AddColumn("dbo.Organizations", "OrganizationEmail", c => c.String());
            AddColumn("dbo.Organizations", "LinkToWebsite", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "LinkToWebsite");
            DropColumn("dbo.Organizations", "OrganizationEmail");
            DropColumn("dbo.Organizations", "PhoneNumber");
            DropColumn("dbo.Organizations", "Address");
        }
    }
}
