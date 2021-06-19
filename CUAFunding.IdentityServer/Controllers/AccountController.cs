using CUAFunding.Common.Exceptions;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.Interfaces.BussinessLogic.Services;
using CUAFunding.ViewModels.AccountViewModel;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CUAFunding.IdentityServer.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        IAccountService _accountService;
        IIdentityServerInteractionService _interactionService;
        ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IIdentityServerInteractionService interactionService)
        {
            _logger = logger;
            _accountService = accountService;
            _interactionService = interactionService;
        }

        public IActionResult Register(string returnUrl)
        {
            RegisterAccountView viewModel = new RegisterAccountView();
            viewModel.CurrentUrl = returnUrl == null ? null : new Uri(returnUrl);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccountView viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _accountService.RegistrationUser(viewModel);

            }
            catch (AuthorizationException ex)
            {
                ModelState.AddModelError(nameof(viewModel.Email), ex.Message);
                foreach (var item in ex.ErrorMessages)
                {
                    ModelState.AddModelError(viewModel.Email, item);

                }
                _logger.LogWarning($"Wrong Registration with email: {viewModel.Email} ErrorMessage: {ex.Message} at : {DateTime.UtcNow}");
                return View(viewModel);
            }

            if (viewModel.CurrentUrl == null) {
               return RedirectToAction("Login");
            }

            return Redirect(viewModel.CurrentUrl.AbsoluteUri);
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginAccountView viewModel = new LoginAccountView();
            viewModel.ReturnUrl = returnUrl;
            viewModel.ExternalProviders = await _accountService.GetExternalProviders();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginAccountView viewModel)
        {
            try
            {
                await _accountService.SignIn(viewModel);
            }
            catch (AuthorizationException ex)
            {
                ModelState.AddModelError(nameof(viewModel.Email), ex.Message);
                _logger.LogWarning($"Wrong Authorization with email: {viewModel.Email} ErrorMessage: {ex.Message} at : {DateTime.UtcNow}");
                viewModel.ExternalProviders = await _accountService.GetExternalProviders();
                return View(viewModel);
            }
            _logger.LogInformation($"User Authorization with email: {viewModel.Email} at : {DateTime.UtcNow}");

            return Redirect(viewModel.ReturnUrl);
        }

        public IActionResult ExternalSignin(string provider, string returnUrl)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _accountService.GetExternalAuthenticationProperties(provider, redirectUri);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            try
            {
                await _accountService.ExternalLogin(result);
            }
            catch (AuthorizationException ex)
            {
                _logger.LogInformation($"Wrong Authorization by ExternalProvider ErrorMessage: {ex.Message} at : {DateTime.UtcNow}");
            }
            _logger.LogInformation($"User Authorization by ExternalProvider with email: {result?.Principal?.Claims?.ToList()?.FirstOrDefault(x => x.Type == ClaimTypes.Email)} at : {DateTime.UtcNow}");

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout(string logoutId)
        {
            string home = "https://localhost:5001/";
            if (!string.IsNullOrEmpty(logoutId))
            {
                var result = await _interactionService.GetLogoutContextAsync(logoutId.ToString());
                await _accountService.SignOut();

                _logger.LogInformation($"User Logout with id: {logoutId} at : {DateTime.UtcNow}");

                if (!string.IsNullOrEmpty(result.PostLogoutRedirectUri))
                {
                    return Redirect(result.PostLogoutRedirectUri);
                }
            }

            return Redirect(home);
        }
    }
}
