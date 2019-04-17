using E_Auction.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T item);
    }
    public interface IAuctionRepository
    {
        void Add(Auction auction);
    }
}
