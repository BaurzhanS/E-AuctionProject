using AutoMapper;
using E_Auction.Core.DataModels;
using E_Auction.Core.Exceptions;
using E_Auction.Core.ViewModels;
using E_Auction.Infrastructure;
using E_Auction.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace E_Auction.BLL.Services
{
    public class AuctionManagementService
    {
        private readonly AplicationDbContext _aplicationDbContext;

        private readonly IAuctionRepository auctionRepository;

        public void OpenAuction(OpenAuctionRequestVm model, int organizationId)
        {
            if (model == null)
                throw new ArgumentNullException($"{typeof(OpenAuctionRequestVm).Name} is null");

            int maximumAllowedActiveAuctions = 3;

            var auctionsCheck = _aplicationDbContext
                .Organizations
                .Find(organizationId)
                .Auctions
                .Where(p => p.AuctionStatus == AuctionStatus.Active)
                .Count() < maximumAllowedActiveAuctions;

            var categoryCheck = _aplicationDbContext.AuctionCategories
                .SingleOrDefault(p => p.Name == model.AuctionCategory);

            if (categoryCheck == null)
                throw new Exception("Ошибка валидации модели!");

            int categoryChecked = categoryCheck.Id;

            if (!auctionsCheck)
                throw new OpenAuctionProcessException(model, "Превышено максимальное количество активных аукционов!");

            Auction auction = new Auction()
            {
                Description = model.Description,
                ShippingAddress = model.ShippingAddress,
                ShippingConditions = model.ShippingConditions,
                PriceAtStart = model.PriceAtStart,
                PriceChangeStep = model.PriceChangeStep,
                PriceAtMinimum = model.PriceAtMinimum,
                StartDate = model.StartDate,
                FinishDateExpected = model.FinishDateExpected,
                AuctionStatus = AuctionStatus.Active,
                AuctionCategoryId = categoryChecked,
                OrganizationId = organizationId
            };

            _aplicationDbContext.Auctions.Add(auction);
            _aplicationDbContext.SaveChanges();

        }

        public void MakeBidToAuction(MakeBidVm model)
        {
            var bidExists = _aplicationDbContext.Bids
                .Any(p => p.Price == model.Price &&
                p.AuctionId == model.AuctionId &&
                p.Description == model.Description &&
                p.OrganizationId == model.OrganizationId);

            if (bidExists)
                throw new Exception("This bid already exists");

            var auctionExistsAndActive = _aplicationDbContext.Auctions.Any(p => p.Id == model.AuctionId && p.OrganizationId == model.OrganizationId);

            if (!auctionExistsAndActive)
                throw new Exception("Данная организация не участвует в активных аукционах");

            var organizationDeposits = _aplicationDbContext.Transactions.Where(p => p.OrganizationId == model.OrganizationId && p.TransactionType == TransactionType.Deposit).ToList();
        
            var organizationWithdrawals = _aplicationDbContext.Transactions.Where(p => p.OrganizationId == model.OrganizationId && p.TransactionType == TransactionType.Withdraw).ToList();
              
            var organizationBalance = organizationDeposits.Sum(p=>p.Sum) - organizationWithdrawals.Sum(p => p.Sum);
            if(organizationBalance<model.Price)
                throw new Exception($"У организации {_aplicationDbContext.Organizations.SingleOrDefault(p=>p.Id==model.OrganizationId).FullName} не хватает средств на балансе");

            var inValidPriceRange = _aplicationDbContext
                .Auctions.Where(p => p.Id == model.AuctionId &&
                p.PriceAtMinimum < model.Price &&
                p.PriceAtStart > model.Price);

            var inStepRange = inValidPriceRange
                .Any(p => (p.PriceAtStart - model.Price) % p.PriceChangeStep == 0);

            if (!inStepRange)
                throw new Exception("Invalid bid according to price step");

            Bid bid = new Bid()
            {
                Price = model.Price,
                Description = model.Description,
                AuctionId = model.AuctionId,
                OrganizationId = model.OrganizationId,
                CreatedDate = DateTime.Now,
                BidStatus=BidStatus.Active
            };
            _aplicationDbContext.Bids.Add(bid);

            Transaction transaction=new Transaction()
            {
                Description="Withdraw for making a bid to take part in the auction",
                OrganizationId=model.OrganizationId,
                Sum=model.Price,
                TransactionDate=DateTime.Now,
                TransactionType=TransactionType.Withdraw
            };
            _aplicationDbContext.Transactions.Add(transaction);

            _aplicationDbContext.SaveChanges();
        }

        public void RevokeBidFromAuction(int BidId)
        {
            var bidExists = _aplicationDbContext.Bids
                .Find(BidId);
            if(bidExists==null)
                throw new Exception("Bid не найден!");
            if ((bidExists.Auction.FinishDateExpected - DateTime.Now).Days < 1)
                throw new Exception("Ставку нельзя удалить! До завершение аукциона осталось менше 24 часов.");
            else
            {
                bidExists.BidStatus = BidStatus.Revoked;
                Transaction deposit = new Transaction()
                {
                    Description = "Пополение за отзыв заявки",
                    OrganizationId = bidExists.OrganizationId,
                    Sum = bidExists.Price,
                    TransactionDate = DateTime.Now,
                    TransactionType = TransactionType.Deposit
                };

                _aplicationDbContext.SaveChanges();
            }
        }

        public IEnumerable<AuctionsInfoVm> GetAuctionsInfo()
        {
            var auctions = _aplicationDbContext.Auctions
                .Include("Bids")
                .Include("Organizations")
                .Select(p => new AuctionsInfoVm()
                {
                    AuctionName = p.Description,
                    BidsCount = p.Bids.Count,
                    BidsTotalAmount = p.Bids.Sum(s=>s.Price),
                    CreatedByOrganization = p.Organizations.FirstOrDefault().FullName
                });

            return auctions.ToList();
        }

        public AuctionDetailedInfoVm GetDetailedAuctionInfo(int auctionId)
        {
            var auction = _aplicationDbContext.Auctions.SingleOrDefault(p => p.Id == auctionId);
            if (auction == null)
                throw new Exception($"Аукциона с номером {auctionId} не существует");
            var auctionFiles = _aplicationDbContext.AuctionFileMeta.Where(p => p.Id == auctionId).ToList();

            AuctionDetailedInfoVm info = new AuctionDetailedInfoVm()
            {
                AuctionId=auction.Id,
                AuctionType=auction.Category.Name,
                AuctionStatus=auction.AuctionStatus.ToString(),
                OrganizationName=auction.Organizations.FirstOrDefault().FullName,
                StartDate=auction.StartDate,
                StartPrice=auction.PriceAtStart,
                MinPrice=auction.PriceAtMinimum,
                PriceStep=auction.PriceChangeStep,
                ShippingAddress=auction.ShippingAddress,
                ShippingConditions=auction.ShippingConditions,
                FinishDate=auction.FinishDateExpected,
                FinishDateAtActual=auction.FinishDateActual,
                AuctionFiles=auctionFiles
            };
            
            return info;
        }

        public void ElectWinnerInAuction(SelectAuctionWinnerVm model)
        {
            var auctionExists = _aplicationDbContext.Auctions.SingleOrDefault(p => p.Id == model.AuctionId);
            if (auctionExists == null)
                throw new Exception("Аукциона под данным номером не имеется");
            var organizationExists = _aplicationDbContext.Organizations.SingleOrDefault(p => p.Id == model.OrganizationId);
            if (organizationExists == null)
                throw new Exception("Организации под данным номером не имеется");
            var bidIsActive = _aplicationDbContext.Bids.SingleOrDefault(p => p.OrganizationId == model.OrganizationId
                && p.AuctionId == model.AuctionId && p.BidStatus == BidStatus.Active);
            if (bidIsActive == null)
                throw new Exception("У компании нет активной заявки на этот аукцион");

            AuctionWinner winner = new AuctionWinner()
            {
                AuctionId=model.AuctionId,
                OrganizationId=model.OrganizationId,
                CreatedAt=DateTime.Now
            };
            _aplicationDbContext.AuctionWinners.Add(winner);

            auctionExists.FinishDateActual = model.CreatedDate;
            auctionExists.AuctionStatus = AuctionStatus.Finished;

            _aplicationDbContext.SaveChanges();
        }

        public AuctionManagementService()
        {
            _aplicationDbContext = new AplicationDbContext();
        }
    }
}
