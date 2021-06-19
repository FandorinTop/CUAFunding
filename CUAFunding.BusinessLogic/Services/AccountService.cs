using CUAFunding.Common.Exceptions;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels.AccountViewModel;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public AccountService(SignInManager<ApplicationUser> singInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = singInManager;
            _userManager = userManager;
        }

        public async Task ExternalLogin(AuthenticateResult result1)
        {
            var result = result1;
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            var externalUser = result.Principal;
            if (externalUser == null)
            {
                throw new Exception("External authentication error");
            }

            var claims = externalUser.Claims.ToList();
            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            }
            if (userIdClaim == null)
            {
                throw new Exception("Unknown userid");
            }

            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var externalUserId = userIdClaim.Value;
            var externalProvider = userIdClaim.Issuer;

            var signInResult = await _signInManager.ExternalLoginSignInAsync(userIdClaim.Issuer, externalUserId, false, false);
            if (signInResult.Succeeded)
            {
                return;
            }
            else
            {
                await RegisterExternal(name.Value, email.Value, new UserLoginInfo(externalProvider, externalUserId, externalProvider));
            }
        }

        public async Task RegisterExternal(string userName, string email, UserLoginInfo info)
        {

            var user = new ApplicationUser()
            {
                UserName = email,
                Email = email,
                NormalizedUserName = userName
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                    var identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                    }
            }
            else
            {
                throw new AuthorizationException("Can`t create ApplicationUser");
            }
        }

        public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfo()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalProviders()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public async Task RegistrationUser(RegisterAccountView viewModel)
        {
            var user = new ApplicationUser();
            user.UserName = viewModel.Email;
            user.Email = viewModel.Email;

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (!result.Succeeded)
            {
                throw new AuthorizationException("Registration error") { ErrorMessages = result.Errors.Select(item => item.Description).ToList()};
            }
            var registredUser = await _userManager.FindByNameAsync(viewModel.Email);
        }

        public async Task SignIn(LoginAccountView viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                throw new AuthorizationException("User is not found");
            }

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, isPersistent: true, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new AuthorizationException("Wrong password");
            }

            var claims = await _userManager.GetClaimsAsync(user);

        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
