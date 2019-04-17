using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Auction.Core.DataModels;

namespace E_Auction.Core.ViewModels
{
    public class FinishAuctionVm
    {
        public int AuctionId { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public DateTime? FinishDateActual { get; set; }
    }
}
