using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public enum AuctionStatus
    {
        Active = 1,
        Finished = 2
    }

    public enum BidStatus
    {
        Active = 1,
        Revoked = 2
    }
    public class Auction
    {
        #region Domain Properties
        public int Id { get; set; }
        public string Description { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingConditions { get; set; }
        public decimal PriceAtStart { get; set; }
        public decimal PriceChangeStep { get; set; }
        public decimal PriceAtMinimum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDateExpected { get; set; }
        public DateTime ? FinishDateActual { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public int AuctionCategoryId { get; set; }
        #endregion

        #region Navigation Properties
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual AuctionCategory Category { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<AuctionFileMeta> Files { get; set; }
        public virtual ICollection<OrganizationRating> OrganizationRatings { get; set; }

        public virtual AuctionWinner AuctionWinner { get; set; }
       
        #endregion
        public Auction()
        {
            Files = new List<AuctionFileMeta>();
            Bids = new List<Bid>();
            OrganizationRatings = new List<OrganizationRating>();
        }
    }

    public class AuctionFileMeta
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte [] ContentAsBase64 { get; set; }
        public DateTime CreatedAt { get; set; }

        public int AuctionId { get; set; }
        public virtual Auction Auction { get; set; }
    }

    public class AuctionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; }

        public AuctionCategory()
        {
            Auctions = new List<Auction>();
        }
    }

    public class Bid
    {
        public int Id { get; set; }   
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public BidStatus BidStatus { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }   
        public int AuctionId { get; set; }
        public virtual Auction Auction { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
   
}
