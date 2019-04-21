using AutoMapper;
using E_Auction.Core.DataModels;
using E_Auction.Core.External;
using E_Auction.Core.ViewModels;
using E_Auction.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.BLL.Services
{
    public class OrganizationManagementService
    {
        private readonly AplicationDbContext _aplicationDbContext;
        private readonly IdentityDbContext _identityDbContext;


        public void OpenOrganization(OpenOrganizationRequestVm model)
        {
            if (model == null)
                throw new ArgumentNullException($"{typeof(OpenOrganizationRequestVm).Name} is null");

            var checkOrganization = /*(from o in _aplicationDbContext.Organizations
                                    where o.IdentificationNumber == model.IdentificationNumber ||
                                    o.FullName == model.FullName
                                    select o).ToList(); */
                                    _aplicationDbContext.Organizations
                                    .SingleOrDefault(p => p.IdentificationNumber == model.IdentificationNumber ||
                                        p.FullName == model.FullName);

            if (checkOrganization != null)
                throw new Exception("Такая организация уже существует в базе");

            var checkOrganizationType = _aplicationDbContext.OrganizationTypes
                .SingleOrDefault(p => p.Name == model.OrganizationType);

            if (checkOrganizationType == null)
                throw new Exception("Организационно-правовая форма организации не корректна");


            var Position = _aplicationDbContext.EmployeePositions.SingleOrDefault(p => p.PositionName == "Director");
            if (Position == null)
                throw new Exception("Данной должности не имеется в списке должностей");

            var UserExists = _identityDbContext.ApplicationUsers.SingleOrDefault(p => p.Email == model.DirectorEmail);
            if (UserExists != null)
                throw new Exception("Пользователь с данным мэйлом уже существует");

            Organization organization = new Organization()
            {
                FullName = model.FullName,
                IdentificationNumber = model.IdentificationNumber,
                RegistrationDate = DateTime.Now,
                OrganizationTypeId = checkOrganizationType.Id
            };
            _aplicationDbContext.Organizations.Add(organization);
            _aplicationDbContext.SaveChanges();

            Employee employee = new Employee()
            {
                FirstName = model.DirectorFirstName,
                LastName = model.DirectorLastName,
                DoB = model.DirectorDoB,
                Email = model.DirectorEmail,
                EmployeePositionId = Position.Id,
                OrganizationId = organization.Id
            };
            _aplicationDbContext.Employees.Add(employee);
            _aplicationDbContext.SaveChanges();

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.DirectorEmail,
                IsActive = true,
                FailedSignInCount = 0,
                CreatedDate = DateTime.Now,
                AssosiatedEmployeeId = employee.Id
            };
            _identityDbContext.ApplicationUsers.Add(user);
            _identityDbContext.SaveChanges();

            ApplicationUserPasswordHistory userPasswordHistory = new ApplicationUserPasswordHistory()
            {
                SetupDate = DateTime.Now,
                InvalidatedDate = DateTime.Now.AddMonths(3),
                Password = model.Password,
                ApplicationUserId = user.Id
            };
            _identityDbContext.ApplicationUserPasswordHistories.Add(userPasswordHistory);
            _identityDbContext.SaveChanges();

            var geoLocationInfo = GeoLocationInfo.GetGeolocationInfo();

            ApplicationUserSignInHistory userSignInHistory = new ApplicationUserSignInHistory()
            {
                SignInTime = DateTime.Now,
                MachineIp = geoLocationInfo.ip,
                IpToGeoCountry = geoLocationInfo.country_name,
                IpToGeoCity = geoLocationInfo.city,
                IpToGeoLatitude = geoLocationInfo.latitude,
                IpToGeoLongitude = geoLocationInfo.longitude,
                ApplicationUserId = user.Id
            };
            _identityDbContext.ApplicationUserSignInHistories.Add(userSignInHistory);
            _identityDbContext.SaveChanges();
        }

        public void RateOrganization(RateOrganizationVm model)
        {
            var organizationExists = _aplicationDbContext.Organizations.SingleOrDefault(p => p.Id == model.OrganizationId);
            if (organizationExists == null)
                throw new Exception("Организации с таким номером не имеется");

            var auctionExists = _aplicationDbContext.Auctions.SingleOrDefault(p => p.Id == model.AuctionId);
            if (auctionExists == null)
                throw new Exception("Аукциона с таким номером не имеется");

            var rateExists = _aplicationDbContext.OrganizationRatings.SingleOrDefault(p => p.OrganizationId == model.OrganizationId
                 && p.AuctionId == model.AuctionId);
            if (rateExists != null)
                throw new Exception("Данная организация уже имеет рейтинг по выбранному аукциону");

            OrganizationRating organizationRating = new OrganizationRating()
            {
                OrganizationId = model.OrganizationId,
                AuctionId = model.AuctionId,
                Point = model.Point
            };
            _aplicationDbContext.OrganizationRatings.Add(organizationRating);
            _aplicationDbContext.SaveChanges();
        }

        public void PerformTransaction(TransactionVm model)
        {
            var organizationExists = _aplicationDbContext.Organizations.SingleOrDefault(p => p.Id == model.OrganizationId);
            if (organizationExists == null)
                throw new Exception("Организации с таким номером не имеется");

            Transaction transaction = new Transaction()
            {
                OrganizationId = model.OrganizationId,
                Sum = model.Sum,
                TransactionDate = DateTime.Now,
                TransactionType = model.TransactionType,
                Description = model.Description
            };
            _aplicationDbContext.Transactions.Add(transaction);
            _aplicationDbContext.SaveChanges();
        }


        public List<Organization> GetFilteredOrganizationsForAuction(FilterOrganizationsForAuctionVm model)
        {
            var OrganizationsBalance = from organization in _aplicationDbContext.Organizations
                                       from auction in organization.Auctions
                                       where auction.Id == model.AuctionId
                                       select new
                                       {
                                           OrganizationId = organization.Id,
                                           OrganizationName = organization.FullName,
                                           Balance = organization.Transactions.Where(p => p.TransactionType == TransactionType.Deposit).Sum(s => s.Sum)
                                             - organization.Transactions.Where(p => p.TransactionType == TransactionType.Withdraw).Sum(s => s.Sum),
                                           AuctionId = auction.Id
                                       };

            var OrganizationsFilteredByBalance = OrganizationsBalance.Where(p => p.Balance >= model.RequiredMinimumAccountBalance);

            var OrganizationsRating = from organization in _aplicationDbContext.Organizations
                                      from rating in organization.OrganizationRatings
                                      where rating.Point >= model.RequiredMinimumOrganizationRating
                                      && rating.AuctionId == model.AuctionId
                                      select new
                                      {
                                          Id = organization.Id,
                                          FullName = organization.FullName
                                      };

           var Organizations =  from orgByBalance in OrganizationsFilteredByBalance
                                from orgByRating in OrganizationsRating
                                where orgByBalance.OrganizationId == orgByRating.Id
                                select new
                                {
                                    Id = orgByBalance.OrganizationId,
                                    FullName = orgByBalance.OrganizationName
                                };

            var FilteredOrganizations = new List<Organization>();

            foreach (var item in Organizations)
            {
                FilteredOrganizations.Add(new Organization()
                {
                    Id=item.Id,
                    FullName=item.FullName
                });
            }

            return FilteredOrganizations;
        }

        public OrganizationManagementService()
        {
            _aplicationDbContext = new AplicationDbContext();
            _identityDbContext = new IdentityDbContext();
        }
    }


}
