using E_Auction.BLL.Services;
using E_Auction.Core.DataModels;
using E_Auction.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.UnitTests.BLL_Tests
{
    [TestClass]
    public class AuctionManagementServiceTests
    {
        /// <summary>
        /// Тест на отзыв ставки с аукциона в случае соблюдения временных сроков
        /// </summary>
        [TestMethod]
        public void AuctionManagementService_RevokeBidFromAuction_ShouldRevokeBid()
        {
            // Tear Up
            AplicationDbContext dbContext = new AplicationDbContext();
            dbContext.Database.CreateIfNotExists();

            var organization = new OrganizationType()
            {
                Name = "Тестовый тип АО",
                Organizations = new List<Organization>()
                {
                    new Organization()
                    {
                        FullName = "Жамантелеком",
                        IdentificationNumber = "-1",
                        RegistrationDate = DateTime.Now
                    }
                }
            };

            dbContext.OrganizationTypes.Add(organization);
            dbContext.SaveChanges();
            var auctionCategory = new AuctionCategory()
            {
                Name = "Тестовые аукционы",
                Description = "Тестовые аукционы",
                Auctions = new List<Auction>()
                {
                    new Auction()
                    {
                        AuctionStatus = AuctionStatus.Active,
                        Description = "Тестовый аукцион",
                        FinishDateActual = null,
                        FinishDateExpected = DateTime.Now.AddDays(3),
                        PriceAtMinimum = 10000,
                        PriceAtStart = 1000000,
                        PriceChangeStep = 10000,
                        ShippingAddress = "Советский союз",
                        ShippingConditions = "Советский союз",
                        StartDate = DateTime.Now.AddDays(-2),
                        OrganizationId = dbContext.Organizations.First().Id,
                        Bids = new List<Bid>()
                        {
                            new Bid()
                            {
                                BidStatus = BidStatus.Active,
                                OrganizationId = dbContext.Organizations.First().Id,
                                Price = 100000,
                                CreatedDate = DateTime.Now.AddHours(-5),
                                Description = "Доставим!"
                            }
                        }
                    }
                }
            };

            dbContext.AuctionCategories.Add(auctionCategory);
            dbContext.SaveChanges();
            // Arrange

            // System Under Test
            var sut = new AuctionManagementService();
            int bidId = dbContext.Bids.ToList().Last().Id;

            // Act
            sut.RevokeBidFromAuction(bidId);

            // Assert
            dbContext = new AplicationDbContext();
            Bid bid = dbContext.Bids.Find(bidId);
            Assert.IsNotNull(bid);
            Assert.IsTrue(bid.BidStatus == BidStatus.Revoked);

            // Tear Down
        }
    }
}
