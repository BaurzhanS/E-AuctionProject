using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Auction.Core.DataModels;

namespace E_Auction.Core.ViewModels
{
    public class TransactionVm
    {
        public int OrganizationId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Sum { get; set; }
        public string Description { get; set; }
    }
}
