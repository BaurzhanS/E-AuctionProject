using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.ViewModels
{
    public class FilterOrganizationsForAuctionVm
    {
        public int AuctionId { get; set; }
        public decimal RequiredMinimumAccountBalance { get; set; }
        public int RequiredMinimumOrganizationRating { get; set; }
    }
}
