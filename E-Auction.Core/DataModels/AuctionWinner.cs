using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class AuctionWinner
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        //[ForeignKey ("Auction")]
        public int AuctionId { get; set; }
        public virtual Auction Auction { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
