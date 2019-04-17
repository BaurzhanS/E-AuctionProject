using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Auction.Core.DataModels;

namespace E_Auction.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AplicationDbContext aplicationDbContext;

        public AuctionRepository()
        {
            aplicationDbContext = new AplicationDbContext();
        }

        public void Add(Auction auction)
        {
            aplicationDbContext.Auctions.Add(auction);
            aplicationDbContext.SaveChanges();
        }
    }
}
