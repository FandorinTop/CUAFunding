using CUAFunding.DomainEntities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CUAFunding.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            return Json(await _userManager.FindByIdAsync(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BanUser(string id)
        {
            (await _userManager.FindByIdAsync(id)).LockoutEnd = DateTime.Now.AddYears(100);

            return Json(true);
        }
    }
}
