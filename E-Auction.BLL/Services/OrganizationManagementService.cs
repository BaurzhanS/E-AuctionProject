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

            Organization organization = new Organization()
            {
                FullName = model.FullName,
                IdentificationNumber = model.IdentificationNumber,
                RegistrationDate = DateTime.Now,
                OrganizationTypeId = checkOrganizationType.Id
            };
            _aplicationDbContext.Organizations.Add(organization);
            _aplicationDbContext.SaveChanges();

            var Position = _aplicationDbContext.EmployeePositions.SingleOrDefault(p => p.PositionName == "Director");
            if(Position==null)
                throw new Exception("Данной должности не имеется в списке должностей");

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
                InvalidatedDate= DateTime.Now.AddMonths(3),
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

            //var rateCheck= _aplicationDbContext.OrganizationRatings.SingleOrDefault(p=>p.OrganizationId==model.OrganizationId
            //    && p.Point)

            OrganizationRating organizationRating = new OrganizationRating()
            {
                OrganizationId=model.OrganizationId,
                Point=model.Point
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
                OrganizationId=model.OrganizationId,
                Sum=model.Sum,
                TransactionDate=DateTime.Now,
                TransactionType=model.TransactionType,
                Description=model.Description
            };

        }

        public OrganizationManagementService()
        {
            _aplicationDbContext = new AplicationDbContext();
            _identityDbContext = new IdentityDbContext();
        }
    }


}
