using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class OrganizationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }

        public OrganizationType()
        {
            Organizations = new List<Organization>();
        }
    }

    public class Organization
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        public int OrganizationTypeId { get; set; }
        public virtual OrganizationType OrganizationType { get; set; }       
        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Bid> Bids { get;set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<AuctionWinner> AuctionWinners { get; set; }
        public virtual ICollection<OrganizationRating> OrganizationRatings { get; set; }

        public Organization()
        {
            Auctions = new List<Auction>();
            Bids = new List<Bid>();
            Transactions = new List<Transaction>();
            AuctionWinners = new List<AuctionWinner>();
            OrganizationRatings = new List<OrganizationRating>();
        }
    }
}
