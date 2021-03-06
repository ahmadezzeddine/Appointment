﻿using System;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using App.Schedule.Context;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using App.Schedule.WebApi.Models;
using System.Collections.Generic;
using App.Schedule.WebApi.Results;
using App.Schedule.WebApi.Services;
using System.Security.Cryptography;
using Microsoft.Owin.Security.OAuth;
using App.Schedule.WebApi.Providers;
using App.Schedule.Domains.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        private readonly AppScheduleDbContext _dbAppointment;

        public AccountController()
        {
            _dbAppointment = new AppScheduleDbContext();
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

       
        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserViewModel model)
        {
            //var result = new ResponseViewModel<tblAdministrator>();

            if (model == null)
                return Ok(new { status = false, data = "", message = "Invalid data model." });

            if (model.UserType == UserType.SiteAdmin)
            {
                var admin = model.SiteAdmin;
                var adminController = new AdministratorController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var checkUser = await UserManager.FindByEmailAsync(admin.Email);
                        if (checkUser != null)
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = model, message = "Email id has already been taken. Please try another email id." });
                        }
                        admin.Password = HttpContext.Current.Server.UrlDecode(admin.Password);
                        var registerViewModel = await adminController.RegisterAdmin(admin, _dbAppointment);
                        if (registerViewModel.Status)
                        {
                            var user = new ApplicationUser() { UserName = admin.Email, Email = admin.Email };
                            var response = await UserManager.CreateAsync(user, admin.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                return Ok(new { status = true, data = registerViewModel, message = "Registration successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false , data = "", message = registerViewModel.Message});
                        }
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        var user = UserManager.FindByEmail(model.SiteAdmin.Email);
                        if (user != null)
                        {
                            UserManager.Delete(user);
                        }
                        return Ok(new { status = false, data = "", message = "There was a problem to register account, Please try again later." });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessAdmin)
            {
                var businessAdmin = model.BusinessAdmin;
                var businessController = new BusinessController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var checkUser = await UserManager.FindByEmailAsync(businessAdmin.Employee.Email);
                        if(checkUser != null)
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = model, message = "Email id has already been taken. Please try another email id." });
                        }
                        businessAdmin.Employee.Password = HttpContext.Current.Server.UrlDecode(businessAdmin.Employee.Password);
                        var registerViewModel = businessController.Register(businessAdmin, _dbAppointment);
                        if (registerViewModel.Status)
                        {
                            var user = new ApplicationUser() { UserName = businessAdmin.Employee.Email, Email = businessAdmin.Employee.Email };
                            var response = await UserManager.CreateAsync(user, businessAdmin.Employee.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                //Send welcome mail
                                var res =await this.SendBusinessWelcomeMail(model.BusinessAdmin.Business);
                                return Ok(new { status = true, data = registerViewModel, message = "Registration successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = registerViewModel, message = registerViewModel.Message != null ? registerViewModel.Message : "Please try again later." });
                        }
                    }
                    catch(Exception ex)
                    {
                        appointmetntDb.Rollback();
                        var user = UserManager.FindByEmail(businessAdmin.Employee.Email);
                        if (user != null)
                        {
                            UserManager.Delete(user);
                        }
                        return Ok(new { status = false, data = "", message = "Please try again later. ex:"+ex.Message.ToString() });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessEmployee)
            {
                var businessEmployee = model.BusinessEmployee;
                var businessEmployeeController = new BusinessEmployeeController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var checkUser = await UserManager.FindByEmailAsync(businessEmployee.Email);
                        if (checkUser != null)
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = model, message = "Email id has already been taken. Please try another email id." });
                        }
                        businessEmployee.Password = HttpContext.Current.Server.UrlDecode(businessEmployee.Password);
                        var registerViewModel =await businessEmployeeController.Register(businessEmployee, _dbAppointment);
                        if (registerViewModel.Status)
                        {
                            var user = new ApplicationUser() { UserName = businessEmployee.Email, Email = businessEmployee.Email };
                            var response = await UserManager.CreateAsync(user, businessEmployee.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                return Ok(new { status = true, data = registerViewModel.Data, message = "Registration successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = "", message = registerViewModel.Message != null ? registerViewModel.Message : "Please try again later." });
                        }
                    }
                    catch(Exception ex)
                    {
                        appointmetntDb.Rollback();
                        var user = UserManager.FindByEmail(businessEmployee.Email);
                        if (user != null)
                        {
                            UserManager.Delete(user);
                        }
                        return Ok(new { status = false, data = "", message = "Please try again later. ex:"+ex.Message.ToString() });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessCustomer)
            {
                var businessCustomer = model.BusinessCustomer;
                var businessCustomerController = new BusinessCustomerController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var checkUser = await UserManager.FindByEmailAsync(businessCustomer.Email);
                        if (checkUser != null)
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = model, message = "Email id has already been taken. Please try another email id." });
                        }
                        businessCustomer.Password = HttpContext.Current.Server.UrlDecode(businessCustomer.Password);
                        var registerViewModel = await businessCustomerController.Register(businessCustomer,_dbAppointment);
                        if (registerViewModel.Status)
                        {
                            var user = new ApplicationUser() { UserName = businessCustomer.Email, Email = businessCustomer.Email };
                            var response = await UserManager.CreateAsync(user, businessCustomer.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                return Ok(new { status = true, data = registerViewModel.Data, message = "Registration successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = registerViewModel, message = registerViewModel.Message != null ? registerViewModel.Message : "Please try again later." });
                        }
                    }
                    catch(Exception ex)
                    {
                        appointmetntDb.Rollback();
                        var user = UserManager.FindByEmail(businessCustomer.Email);
                        if (user != null)
                        {
                            UserManager.Delete(user);
                        }
                        return Ok(new { status = false, data = "", message = "Please try again later. ex:"+ ex.Message.ToString()});
                    }
                }
            }
            else
            {
                return Ok(new { status = false, data = "", message = "Invalid user." });
            }
        }


        // PUT api/Account/UpdateAdmin
        [HttpPut]
        [Route("UpdateUser")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateUser(UserViewModel model)
        {
            if (model == null)
                return Ok(new { status = false, data = "", message = "Invalid data model." });

            if (model.UserType == UserType.SiteAdmin)
            {
                var admin = model.SiteAdmin;
                var adminController = new AdministratorController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        admin.Password = HttpContext.Current.Server.UrlDecode(admin.Password);
                        var updateAdmin = adminController.UpdateAdmin(admin, _dbAppointment);
                        if (updateAdmin.Status)
                        {
                            var user = await UserManager.FindByEmailAsync(admin.Email);
                            var response = await UserManager.ChangePasswordAsync(user.Id, admin.OldPassword, admin.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                return Ok(new { status = true, data = updateAdmin.Data, message = "update successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = "", message = updateAdmin.Message });
                        }
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = "There was a problem Please try again later." });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessAdmin || model.UserType == UserType.BusinessEmployee)
            {
                var employe = model.BusinessEmployee;
                var businessEmployeeController = new BusinessEmployeeController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        employe.Password = HttpContext.Current.Server.UrlDecode(employe.Password);
                        var updateAdmin = businessEmployeeController.UpdateEmployee(employe, _dbAppointment);
                        if (updateAdmin.Status)
                        {
                            var user = await UserManager.FindByEmailAsync(employe.Email);
                            var response = await UserManager.ChangePasswordAsync(user.Id, employe.OldPassword, employe.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                return Ok(new { status = true, data = updateAdmin.Data, message = "changed successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            
                            return Ok(new { status = false, data = "", message = updateAdmin.Message });
                        }
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = "There was a problem Please try again later." });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessCustomer)
            {
                var customer = model.BusinessCustomer;
                var businessCustomerController = new BusinessCustomerController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        customer.Password = HttpContext.Current.Server.UrlDecode(customer.Password);
                        var updateCustomer = businessCustomerController.UpdateCustomer(customer, _dbAppointment);
                        if (updateCustomer.Status)
                        {
                            var user = await UserManager.FindByEmailAsync(customer.Email);
                            var response = await UserManager.ChangePasswordAsync(user.Id, customer.OldPassword, customer.Password);
                            if (response.Succeeded)
                            {
                                appointmetntDb.Commit();
                                return Ok(new { status = true, data = updateCustomer.Data, message = "changed successfully." });
                            }
                            else
                            {
                                appointmetntDb.Rollback();
                                return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                            }
                        }
                        else
                        {
                            appointmetntDb.Rollback();
                            return Ok(new { status = false, data = "", message = updateCustomer.Message });
                        }
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = "There was a problem Please try again later." });
                    }
                }
            }
            else
            {
                return Ok(new { status = false, data = "", message = "Invalid user." });
            }
        }

        // PUT api/Account/DeleteUser
        [HttpPost]
        [Route("DeleteUser")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> DeleteUser(UserViewModel model)
        {
            if (model == null)
                return Ok(new { status = false, data = "", message = "Invalid data model." });

            if(model.UserType == UserType.SiteAdmin)
            {
                var siteAdmin = model.SiteAdmin;
                var administratorController = new AdministratorController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var deleteEmploye = administratorController.DeleteSiteAdmin(siteAdmin.Id, _dbAppointment);
                        if (deleteEmploye.Status)
                        {
                            var user = await UserManager.FindByEmailAsync(deleteEmploye.Data.Email);
                            if (user != null)
                            {
                                var response = await UserManager.DeleteAsync(user);
                                if (!response.Succeeded)
                                {
                                    appointmetntDb.Rollback();
                                    return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                                }
                            }
                            appointmetntDb.Commit();
                            return Ok(new { status = true, data = deleteEmploye.Data, message = "deleted successfully." });
                        }
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = deleteEmploye.Message });
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = "There was a problem Please try again later." });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessEmployee)
            {
                var employe = model.BusinessEmployee;
                var businessEmployeeController = new BusinessEmployeeController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var deleteEmploye = businessEmployeeController.DeleteEmployee(employe.Id, _dbAppointment);
                        if (deleteEmploye.Status)
                        {
                            var user = await UserManager.FindByEmailAsync(deleteEmploye.Data.Email);
                            if (user != null)
                            {
                                var response = await UserManager.DeleteAsync(user);
                                if (!response.Succeeded)
                                {
                                    appointmetntDb.Rollback();
                                    return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                                }
                            }
                            appointmetntDb.Commit();
                            return Ok(new { status = true, data = deleteEmploye.Data, message = "deleted successfully." });
                        }
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = deleteEmploye.Message });
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = "There was a problem Please try again later." });
                    }
                }
            }
            else if (model.UserType == UserType.BusinessCustomer)
            {
                var customer = model.BusinessCustomer;
                var businessCustomerController = new BusinessCustomerController();
                using (var appointmetntDb = _dbAppointment.Database.BeginTransaction())
                {
                    try
                    {
                        var deleteCustomer = businessCustomerController.DeleteCustomer(customer.Id, _dbAppointment);
                        if (deleteCustomer.Status)
                        {
                            var user = await UserManager.FindByEmailAsync(deleteCustomer.Data.Email);
                            if (user != null)
                            {
                                var response = await UserManager.DeleteAsync(user);
                                if (!response.Succeeded)
                                {
                                    appointmetntDb.Rollback();
                                    return Ok(new { status = false, data = "", message = response.Errors.SingleOrDefault() });
                                }
                            }
                            appointmetntDb.Commit();
                            return Ok(new { status = true, data = deleteCustomer.Data, message = "deleted successfully." });
                        }
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = deleteCustomer.Message });
                    }
                    catch
                    {
                        appointmetntDb.Rollback();
                        return Ok(new { status = false, data = "", message = "There was a problem Please try again later." });
                    }
                }
            }
            else
            {
                return Ok(new { status = false, data = "", message = "Invalid user." });
            }
        }

        [NonAction]
        private async Task<MailResponse> SendBusinessWelcomeMail(BusinessViewModel model)
        {
            var mailService = new MailService();
            var toMail = new List<string>();
            if (String.IsNullOrEmpty(model.Email))
            {
                toMail.Add(model.Email);
            }
            var htmlMailBody = new StringBuilder();
            htmlMailBody.Append("<div>");
            htmlMailBody.Append("<div>Hi, "+model.Name+"</div><br /><br />");
            htmlMailBody.Append("<div><h3>Successfully created account.</h3></div><br />");
            htmlMailBody.Append("<div><p>There are lots of things you can do with appointment scheduler. it will track your business day to day meeting. and so on.. <a href='#'>read more...</a></p></div><br />");
            htmlMailBody.Append("<br /><br /><br />");
            htmlMailBody.Append("<h4>Regard's</h4>");
            htmlMailBody.Append("<h3>Appointment Scheduler</h3>");
            htmlMailBody.Append("</div>");

            var mailInfomration = new MailInformation()
            {
                To = toMail,
                Subject = "Appointment Scheduler, Site admin login id and password",
                HtmlText = htmlMailBody.ToString(),
                PlainText = ""
            };
            return await mailService.SendMail(mailInfomration);
        }
    }
}
