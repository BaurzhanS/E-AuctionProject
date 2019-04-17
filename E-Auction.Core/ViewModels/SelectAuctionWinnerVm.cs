using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Auction.Core.DataModels;

namespace E_Auction.Core.ViewModels
{
    public class SelectAuctionWinnerVm
    {
        public int OrganizationId { get; set; }
        public int AuctionId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
