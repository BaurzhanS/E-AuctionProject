using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class ApplicationUserPasswordHistory
    {
        public int Id { get; set; }
        public DateTime SetupDate { get; set; }
        public DateTime InvalidatedDate { get; set; }
        public string Password { get; set; }

        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
