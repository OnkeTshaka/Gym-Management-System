namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingTrainer",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        BookingDate = c.DateTime(nullable: false),
                        Username = c.String(),
                        Email = c.String(),
                        IDNum = c.String(),
                        Address = c.String(),
                        MobileNumber = c.String(),
                        EventID = c.Int(nullable: false),
                        memberID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Event", t => t.EventID, cascadeDelete: true)
                .ForeignKey("dbo.Member", t => t.memberID, cascadeDelete: true)
                .Index(t => t.EventID)
                .Index(t => t.memberID);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 100),
                        Description = c.String(maxLength: 300),
                        Link = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(),
                        ThemeColor = c.String(maxLength: 10),
                        IsFullDay = c.Boolean(nullable: false),
                        Trainer = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Username = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        IDNum = c.String(),
                        MobileNumber = c.String(nullable: false),
                        JoinDate = c.String(nullable: false),
                        Profile = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MembershipPlan",
                c => new
                    {
                        PlanID = c.Int(nullable: false, identity: true),
                        Shift = c.String(),
                        JoinDate = c.String(),
                        Username = c.String(),
                        Email = c.String(),
                        IDNum = c.String(),
                        Address = c.String(),
                        CancelMember = c.Boolean(nullable: false),
                        Description = c.String(),
                        MobileNumber = c.String(),
                        Monthly_Fee = c.Double(nullable: false),
                        Joining_Fee = c.Double(nullable: false),
                        TotalCost = c.Double(nullable: false),
                        trialPeriod = c.DateTime(nullable: false),
                        PackId = c.Int(nullable: false),
                        memberID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlanID)
                .ForeignKey("dbo.Member", t => t.memberID, cascadeDelete: true)
                .ForeignKey("dbo.Package", t => t.PackId, cascadeDelete: true)
                .Index(t => t.PackId)
                .Index(t => t.memberID);
            
            CreateTable(
                "dbo.Package",
                c => new
                    {
                        PackId = c.Int(nullable: false, identity: true),
                        PackageType = c.String(),
                        Description = c.String(nullable: false),
                        Duration = c.Int(nullable: false),
                        Monthly_Fee = c.Double(nullable: false),
                        Joining_Fee = c.Double(nullable: false),
                        TotalCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PackId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.CommentsRating",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        ThisDateTime = c.DateTime(),
                        ArticleId = c.Int(nullable: false),
                        Rating = c.Int(),
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Member", t => t.ID, cascadeDelete: true)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.ContactUsForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        EmailAddress = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Message = c.String(nullable: false, maxLength: 500),
                        TimeStamp = c.String(nullable: false),
                        IsReaded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeliveryReturn",
                c => new
                    {
                        DeliveryReturnID = c.Int(nullable: false, identity: true),
                        ReturnItemID = c.Int(nullable: false),
                        DriverID = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        FromTime = c.DateTime(nullable: false),
                        ToTime = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.DeliveryReturnID)
                .ForeignKey("dbo.Driver", t => t.DriverID, cascadeDelete: true)
                .ForeignKey("dbo.ReturnItem", t => t.ReturnItemID, cascadeDelete: true)
                .Index(t => t.ReturnItemID)
                .Index(t => t.DriverID);
            
            CreateTable(
                "dbo.Driver",
                c => new
                    {
                        DriverID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IDNumber = c.String(nullable: false, maxLength: 14),
                        DriverLicence = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        EmailAddress = c.String(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DriverID);
            
            CreateTable(
                "dbo.ReturnItem",
                c => new
                    {
                        ReturnItemID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        ClientEmail = c.String(),
                        ReturnDate = c.DateTime(nullable: false),
                        selectAction = c.Int(nullable: false),
                        Status = c.String(),
                        From = c.String(),
                        To = c.String(),
                        memberID = c.Int(nullable: false),
                        ReasonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReturnItemID)
                .ForeignKey("dbo.Member", t => t.memberID, cascadeDelete: true)
                .ForeignKey("dbo.Reason", t => t.ReasonID, cascadeDelete: true)
                .Index(t => t.memberID)
                .Index(t => t.ReasonID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Picture = c.Binary(),
                        Price = c.Double(nullable: false),
                        Time = c.String(),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        ReturnItem_ReturnItemID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ReturnItem", t => t.ReturnItem_ReturnItemID)
                .Index(t => t.CategoryID)
                .Index(t => t.ReturnItem_ReturnItemID);
            
            CreateTable(
                "dbo.Reason",
                c => new
                    {
                        ReasonID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ReasonID);
            
            CreateTable(
                "dbo.DeliveryTimes",
                c => new
                    {
                        DeliveryTimesID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        DriverID = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        FromTime = c.DateTime(nullable: false),
                        ToTime = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.DeliveryTimesID)
                .ForeignKey("dbo.Driver", t => t.DriverID, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.DriverID);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        CustomerEmail = c.String(),
                        CustomerAddress = c.String(),
                        Refcode = c.String(),
                        From = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        PaymentType = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Member_ID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Member", t => t.Member_ID)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID)
                .Index(t => t.Member_ID);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        FeedbackID = c.Int(nullable: false, identity: true),
                        Reasons = c.String(),
                        Other = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.FeedbackID);
            
            CreateTable(
                "dbo.RateBooking",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        ThisDateTime = c.DateTime(),
                        ArticleId = c.Int(nullable: false),
                        Rating = c.Int(),
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Member", t => t.ID, cascadeDelete: true)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.RateClass",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        ThisDateTime = c.DateTime(),
                        ArticleId = c.Int(nullable: false),
                        Rating = c.Int(),
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClassId)
                .ForeignKey("dbo.Member", t => t.ID, cascadeDelete: true)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.RequestMember",
                c => new
                    {
                        RequestMemberID = c.Int(nullable: false, identity: true),
                        JoinDate = c.String(),
                        ApplicationDate = c.String(),
                        Username = c.String(),
                        Email = c.String(),
                        Responses = c.Int(nullable: false),
                        Description = c.String(),
                        TotalCost = c.Double(nullable: false),
                        Apply = c.Boolean(nullable: false),
                        Period = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RequestMemberID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        SessionID = c.Int(nullable: false, identity: true),
                        SessionType = c.String(),
                        Icon = c.Binary(),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SessionID);
            
            CreateTable(
                "dbo.supplierCategory",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        IsActive = c.Boolean(),
                        IsDelete = c.Boolean(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.supplierProduct",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        CustomerEmail = c.String(),
                        CustomerAddress = c.String(),
                        CategoryID = c.Int(),
                        IsActive = c.Boolean(),
                        IsDelete = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Description = c.String(),
                        ProductImage = c.String(),
                        IsFeatured = c.Boolean(),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.supplierCategory", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.myCart",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(),
                        MemberID = c.Int(nullable: false),
                        CartStatusID = c.Int(),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.supplierProduct", t => t.ProductID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.SupplierClass",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        InvoicePrice = c.Double(nullable: false),
                        To = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.SupplierID);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        TermsID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Agreed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TermsID);
            
            CreateTable(
                "dbo.Trainer",
                c => new
                    {
                        TrainerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastName = c.String(),
                        Contact_No = c.String(),
                        Experience = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        Picture = c.Binary(),
                        ID_No = c.String(),
                        SessionName = c.String(),
                        ClassDescription = c.String(),
                        DOB = c.Int(nullable: false),
                        age = c.Int(nullable: false),
                        gender = c.String(),
                        century = c.String(),
                        SessionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrainerID)
                .ForeignKey("dbo.Session", t => t.SessionID, cascadeDelete: true)
                .Index(t => t.SessionID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Address = c.String(),
                        IDNum = c.String(),
                        LastName = c.String(),
                        ProfilePicture = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainer", "SessionID", "dbo.Session");
            DropForeignKey("dbo.supplierProduct", "CategoryID", "dbo.supplierCategory");
            DropForeignKey("dbo.myCart", "ProductID", "dbo.supplierProduct");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RateClass", "ID", "dbo.Member");
            DropForeignKey("dbo.RateBooking", "ID", "dbo.Member");
            DropForeignKey("dbo.DeliveryTimes", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderDetail", "ProductID", "dbo.Product");
            DropForeignKey("dbo.OrderDetail", "OrderID", "dbo.Order");
            DropForeignKey("dbo.OrderDetail", "Member_ID", "dbo.Member");
            DropForeignKey("dbo.DeliveryTimes", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.DeliveryReturn", "ReturnItemID", "dbo.ReturnItem");
            DropForeignKey("dbo.ReturnItem", "ReasonID", "dbo.Reason");
            DropForeignKey("dbo.Product", "ReturnItem_ReturnItemID", "dbo.ReturnItem");
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.ReturnItem", "memberID", "dbo.Member");
            DropForeignKey("dbo.DeliveryReturn", "DriverID", "dbo.Driver");
            DropForeignKey("dbo.CommentsRating", "ID", "dbo.Member");
            DropForeignKey("dbo.BookingTrainer", "memberID", "dbo.Member");
            DropForeignKey("dbo.MembershipPlan", "PackId", "dbo.Package");
            DropForeignKey("dbo.MembershipPlan", "memberID", "dbo.Member");
            DropForeignKey("dbo.BookingTrainer", "EventID", "dbo.Event");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Trainer", new[] { "SessionID" });
            DropIndex("dbo.myCart", new[] { "ProductID" });
            DropIndex("dbo.supplierProduct", new[] { "CategoryID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RateClass", new[] { "ID" });
            DropIndex("dbo.RateBooking", new[] { "ID" });
            DropIndex("dbo.OrderDetail", new[] { "Member_ID" });
            DropIndex("dbo.OrderDetail", new[] { "ProductID" });
            DropIndex("dbo.OrderDetail", new[] { "OrderID" });
            DropIndex("dbo.DeliveryTimes", new[] { "DriverID" });
            DropIndex("dbo.DeliveryTimes", new[] { "OrderID" });
            DropIndex("dbo.Product", new[] { "ReturnItem_ReturnItemID" });
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropIndex("dbo.ReturnItem", new[] { "ReasonID" });
            DropIndex("dbo.ReturnItem", new[] { "memberID" });
            DropIndex("dbo.DeliveryReturn", new[] { "DriverID" });
            DropIndex("dbo.DeliveryReturn", new[] { "ReturnItemID" });
            DropIndex("dbo.CommentsRating", new[] { "ID" });
            DropIndex("dbo.MembershipPlan", new[] { "memberID" });
            DropIndex("dbo.MembershipPlan", new[] { "PackId" });
            DropIndex("dbo.BookingTrainer", new[] { "memberID" });
            DropIndex("dbo.BookingTrainer", new[] { "EventID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Trainer");
            DropTable("dbo.Terms");
            DropTable("dbo.SupplierClass");
            DropTable("dbo.myCart");
            DropTable("dbo.supplierProduct");
            DropTable("dbo.supplierCategory");
            DropTable("dbo.Session");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RequestMember");
            DropTable("dbo.RateClass");
            DropTable("dbo.RateBooking");
            DropTable("dbo.Feedback");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Order");
            DropTable("dbo.DeliveryTimes");
            DropTable("dbo.Reason");
            DropTable("dbo.Product");
            DropTable("dbo.ReturnItem");
            DropTable("dbo.Driver");
            DropTable("dbo.DeliveryReturn");
            DropTable("dbo.ContactUsForm");
            DropTable("dbo.CommentsRating");
            DropTable("dbo.Category");
            DropTable("dbo.Package");
            DropTable("dbo.MembershipPlan");
            DropTable("dbo.Member");
            DropTable("dbo.Event");
            DropTable("dbo.BookingTrainer");
        }
    }
}
