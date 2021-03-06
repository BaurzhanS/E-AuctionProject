﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.ViewModels
{
    public class OpenOrganizationRequestVm
    {
        public string FullName { get; set; }
        public string IdentificationNumber { get; set; }
        public string OrganizationType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string OrganizationEmail { get; set; }
        public string LinkToWebsite { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string DirectorFirstName { get; set; }
        public string DirectorLastName { get; set; }
        public string DirectorEmail { get; set; }
        public DateTime DirectorDoB { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
