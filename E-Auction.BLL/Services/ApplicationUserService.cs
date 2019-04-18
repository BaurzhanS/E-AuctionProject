using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Auction.Core.DataModels;
using E_Auction.Core.External;
using E_Auction.Core.ViewModels;
using E_Auction.Infrastructure;

namespace E_Auction.BLL.Services
{
    public class ApplicationUserService
    {
        private readonly IdentityDbContext _identityDbContext;

        public void ValidateUserInLogIn(UserLogOnVm model)
        {
            var userExists = _identityDbContext.ApplicationUsers.SingleOrDefault(p => p.Email == model.Email);
            if (userExists == null)
                throw new Exception("Пользователь с таким мэйлом отсутствует в системе");

            var passwordCheck = _identityDbContext.ApplicationUserPasswordHistories
                .SingleOrDefault(p => p.ApplicationUserId == userExists.Id && p.Password == model.Password);
            if (passwordCheck == null)
            {
                userExists.FailedSignInCount++;
                _identityDbContext.SaveChanges();
                throw new Exception("У данного пользователя другой пароль");
            }

            var FailedSignInCount = _identityDbContext.ApplicationUsers.SingleOrDefault(p=>p.Email==model.Email).FailedSignInCount;

            if (FailedSignInCount > 5)
            {
                userExists.IsActive = false;
                throw new Exception("Вы ввели неправильный пароль более 5 раз");
            }
            var invalidatedDateCheck = _identityDbContext.ApplicationUserPasswordHistories
                .Where(p => p.ApplicationUser.Email == model.Email && p.Password == model.Password)
                .OrderByDescending(p => p.SetupDate).ToList();
            if (invalidatedDateCheck[0].InvalidatedDate <= DateTime.Now)
            {
                userExists.IsActive = false;
                throw new Exception("Истекла валидная дата для данного пароля");
                _identityDbContext.SaveChanges();
            }


            var validUser = _identityDbContext.ApplicationUsers.SingleOrDefault(p =>p.Id==userExists.Id && (p.IsActive == true));
            if (validUser == null)
                throw new Exception("Данный пользователь заблокирован");

            validUser.FailedSignInCount = 0;
            _identityDbContext.SaveChanges();

            GeoLocationInfo geoInfo = GeoLocationInfo.GetGeolocationInfo();

            ApplicationUserSignInHistory userSignInHistory = new ApplicationUserSignInHistory()
            {
                ApplicationUserId = validUser.Id,
                IpToGeoCity = geoInfo.city,
                IpToGeoLatitude=geoInfo.latitude,
                IpToGeoCountry=geoInfo.country_name,
                IpToGeoLongitude=geoInfo.longitude,
                MachineIp=geoInfo.ip,
                SignInTime=DateTime.Now
            };

            _identityDbContext.ApplicationUserSignInHistories.Add(userSignInHistory);
            _identityDbContext.SaveChanges();
        }

        public void ChangeUserPassword(ChangePasswordVm model)
        {
            var passwordCheck = _identityDbContext.ApplicationUserPasswordHistories
                .SingleOrDefault(p => p.ApplicationUser.Email == model.UserEmail && p.Password == model.UserPassword);
            if(passwordCheck==null)
                throw new Exception("Неправильный пароль");

            var userPasswordHistory = _identityDbContext.ApplicationUserPasswordHistories
                .Where(p => p.ApplicationUser.Email == model.UserEmail).ToList();

            bool flag = true;
            if (userPasswordHistory.Count() < 5)
            {
                foreach (var item in userPasswordHistory)
                {
                    if (item.Password == model.UserNewPassword)
                        flag = false;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if(userPasswordHistory[i].Password== model.UserNewPassword)
                        flag = false;
                }
            }
            if(!flag)
                throw new Exception("Данный пароль уже использовался выберите другой");

            if(model.UserNewPassword!=model.UserNewPasswordConfirmed)
                throw new Exception("Новый пароль не соответствует подтвержденному");

            ApplicationUserPasswordHistory ApplicationUserPasswordHistory = new ApplicationUserPasswordHistory()
            {
                ApplicationUserId = userPasswordHistory[0].ApplicationUser.Id,
                Password = model.UserNewPassword,
                SetupDate = DateTime.Now,
                InvalidatedDate = DateTime.Now.AddMonths(3)
            };

            _identityDbContext.ApplicationUserPasswordHistories.Add(ApplicationUserPasswordHistory);

            var userIsActive = _identityDbContext.ApplicationUsers.SingleOrDefault(p => p.Email == model.UserEmail
             && p.IsActive == false);

            if (userIsActive != null)
                userIsActive.IsActive = true;
            
            _identityDbContext.SaveChanges();
        }

        public ApplicationUserService()
        {
            _identityDbContext = new IdentityDbContext();
        }
    }
}
