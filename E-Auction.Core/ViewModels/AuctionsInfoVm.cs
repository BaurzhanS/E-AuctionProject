using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Auction.Core.DataModels;

namespace E_Auction.Core.ViewModels
{
    public class AuctionsInfoVm
    {
        public string AuctionName { get; set; }
        public string CreatedByOrganization { get; set; }
        public int BidsCount { get; set; }
        public decimal ? BidsTotalAmount { get; set; }
    }
}
