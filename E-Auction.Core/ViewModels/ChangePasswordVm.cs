using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.ViewModels
{
    public class ChangePasswordVm
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserNewPassword { get; set; }
        public string UserNewPasswordConfirmed { get; set; }
    }
}
