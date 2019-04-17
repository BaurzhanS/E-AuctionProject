using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class OrganizationRating
    {
        public int Id { get; set; }
        public int Point { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int AuctionId { get; set; }
        public virtual Auction Auction { get; set; }
    }
}
