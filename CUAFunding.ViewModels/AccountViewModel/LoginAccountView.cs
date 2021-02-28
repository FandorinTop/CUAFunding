using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Text;

namespace CUAFunding.ViewModels.AccountViewModel
{
    public class LoginAccountView
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}
