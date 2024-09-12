using CUAFunding.ViewModels.AccountViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUAFunding.Interfaces.BussinessLogic.Services
{
    public interface IAccountService
    {
        AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl);
        Task ExternalLogin(AuthenticateResult returnUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfo();
        Task<IEnumerable<AuthenticationScheme>> GetExternalProviders();
        Task RegistrationUser(RegisterAccountView viewModel);
        Task SignIn(LoginAccountView viewModel);
        Task SignOut();
    }
}
