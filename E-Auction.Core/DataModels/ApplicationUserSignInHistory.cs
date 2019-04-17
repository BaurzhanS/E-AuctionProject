using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class ApplicationUserSignInHistory
    {
        public int Id { get; set; }
        public DateTime SignInTime { get; set; }
        public string MachineIp { get; set; }
        public string IpToGeoCountry { get; set; }
        public string IpToGeoCity { get; set; }
        public double IpToGeoLatitude { get; set; }
        public double IpToGeoLongitude { get; set; }

        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
