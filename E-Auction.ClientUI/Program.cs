using AutoMapper;
using E_Auction.BLL.Mappers;
using E_Auction.BLL.Services;
using E_Auction.Core.DataModels;
using E_Auction.Core.Exceptions;
using E_Auction.Core.ViewModels;
using E_Auction.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace E_Auction.ClientUI
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\Академия шаг\ADO.NET\Пример.txt";
            
            //AplicationDbContext dbContext = new AplicationDbContext();
            //AuctionManagementService service = new AuctionManagementService();
            //service.OpenAuction(new OpenAuctionRequestVm()
            //{
            //    AuctionCategory = "Техника",
            //    Description = "Доставка канцелярских товаров",
            //    FinishDateExpected = DateTime.Now.AddMonths(3),
            //    PriceAtMinimum = 30000,
            //    PriceAtStart = 100000,
            //    PriceChangeStep = 5000,
            //    ShippingAddress = "Маркова 5, Алматы",
            //    ShippingConditions = "До офиса",
            //    StartDate = DateTime.Now
            //}, 1);

            //service.MakeBidToAuction(new MakeBidVm()
            //{
            //    AuctionId = 1,
            //    OrganizationId = 1,
            //    Price = 570000,
            //    Description = "Цена доставки техники"
            //});

            //var auctions = service.GetAuctionsInfo();
            //foreach (var item in auctions)
            //{
            //    Console.WriteLine($"{item.AuctionName} {item.CreatedByOrganization} {item.BidsCount} {item.BidsTotalAmount}");
            //}

            //var auctions = service.GetDetailedAuctionInfo(2);
            //Console.WriteLine(auctions.AuctionId);
            //Console.WriteLine(auctions.AuctionType);
            //Console.WriteLine(auctions.AuctionStatus);
            //Console.WriteLine(auctions.FinishDate);
            //Console.WriteLine(auctions.FinishDateAtActual);
            //Console.WriteLine(auctions.MinPrice);
            //Console.WriteLine(auctions.OrganizationName);
            //Console.WriteLine(auctions.PriceStep);
            //Console.WriteLine(auctions.ShippingAddress);
            //Console.WriteLine(auctions.ShippingConditions);
            //Console.WriteLine(auctions.StartDate);
            //Console.WriteLine(auctions.AuctionFiles);

            //service.ElectWinnerInAuction(new SelectAuctionWinnerVm()
            //{
            //    AuctionId = 1,
            //    OrganizationId = 1,
            //    CreatedDate = DateTime.Now
            //});
            //service.FinishAuction(new FinishAuctionVm()
            //{
            //    AuctionId=1,
            //    AuctionStatus=AuctionStatus.Finished,
            //    FinishDateActual=DateTime.Now
            //});
            //Console.ReadLine();

            OrganizationManagementService org = new OrganizationManagementService();
            //org.OpenOrganization(new OpenOrganizationRequestVm()
            //{
            //    FullName = "Казахтелеком",
            //    IdentificationNumber = "123456",
            //    OrganizationType = "АО",
            //    DirectorFirstName = "Бахыт",
            //    DirectorLastName = "Султанов",
            //    DirectorEmail = "baha@mail.ru",
            //    DirectorDoB = DateTime.Now.AddYears(-28),
            //    Password = "12345",
            //    PasswordConfirm = "12345"
            //});

            //org.RateOrganization(new RateOrganizationVm()
            //{
            //    OrganizationId = 1,
            //    Point = 4,
            //    AuctionId = 1
            //});

            //org.PerformTransaction(new TransactionVm()
            //{
            //    Description="Пополнение счета",
            //    OrganizationId=1,
            //    Sum=600000,
            //    TransactionType=TransactionType.Deposit
            //});

            ApplicationUserService service = new ApplicationUserService();
            //service.ValidateUserInLogIn(new UserLogOnVm()
            //{
            //    Email= "baha@mail.ru",
            //    Password = "123"
            //});
            //service.ChangeUserPassword(new ChangePasswordVm()
            //{
            //    UserEmail = "baha@mail.ru",
            //    UserPassword="123",
            //    UserNewPassword="12345",
            //    UserNewPasswordConfirmed="12345"
            //});
        }
    }
}
