using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int FailedSignInCount { get; set; }
        public int AssosiatedEmployeeId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<ApplicationUserPasswordHistory> ApplicationUserPasswordHistories { get; set; }
        public virtual ICollection<ApplicationUserSignInHistory> ApplicationUserSignInHistories { get; set; }

        public ApplicationUser()
        {
            ApplicationUserPasswordHistories = new List<ApplicationUserPasswordHistory>();
            ApplicationUserSignInHistories = new List<ApplicationUserSignInHistory>();
        }
    }
}
