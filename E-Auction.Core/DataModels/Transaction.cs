using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public enum TransactionType { Deposit=1, Withdraw }
    public class Transaction
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Sum { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
