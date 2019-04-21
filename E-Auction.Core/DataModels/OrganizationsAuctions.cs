using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class OrganizationsAuctions
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int AuctionId { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Auction Auction { get; set; }

        public OrganizationsAuctions()
        {
            Organizations = new List<Organization>();
            Auctions = new List<Auction>();
            Transactions = new List<Transaction>();
        }
    }
}
