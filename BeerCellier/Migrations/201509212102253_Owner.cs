namespace BeerCellier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beers", "Owner_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Beers", "Owner_ID");
            AddForeignKey("dbo.Beers", "Owner_ID", "dbo.Users", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Beers", "Owner_ID", "dbo.Users");
            DropIndex("dbo.Beers", new[] { "Owner_ID" });
            DropColumn("dbo.Beers", "Owner_ID");
        }
    }
}
