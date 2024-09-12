using CUAFunding.Common.Helpers;
using CUAFunding.DataAccess;
using CUAFunding.DomainEntities.Entities;
using CUAFunding.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CUAFunding.IdentityServer.Controllers
{
    public class UsersInfo
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public int ProjectCount { get; set; }

        public string Role { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<UserController> logger, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ApiResult<UsersInfo>> GetUsersData(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var source = _context.ApplicationUsers.Select(item => new UsersInfo
            {
                UserName = item.UserName,
                Email = item.Email,
                ProjectCount = item.Chats.Count()
            });


            if (!String.IsNullOrEmpty(filterColumn) && !String.IsNullOrEmpty(filterQuery) && typeof(ApplicationUser).IsValidProperty(filterColumn))
            {
                source = source.Where(String.Format("{0}.Contains(@0)", filterColumn), filterQuery);
            }

            if (!String.IsNullOrEmpty(sortColumn) && typeof(ApplicationUser).IsValidProperty(sortColumn))
            {
                var sortingString = String.Empty;

                if (!string.IsNullOrEmpty(sortColumn)
                       && typeof(ApplicationUser).IsValidProperty(sortColumn))
                {
                    sortOrder = !string.IsNullOrEmpty(sortOrder)
                        && sortOrder.ToUpper() == "ASC"
                        ? "ASC"
                        : "DESC";

                    if (string.IsNullOrEmpty(sortingString))
                    {
                        sortingString += $"{sortColumn} {sortOrder}";
                    }
                }

                source = source.OrderBy(sortingString);
            }

            var Projects = await _context.Projects.ToListAsync();

            var skipedSource = source.Skip(pageIndex * pageSize).Take(pageSize);
            var count = await source.CountAsync();

            var data = await skipedSource.ToListAsync();

            foreach (var item in data)
            {
                var user = await _userManager.FindByEmailAsync(item.Email);
                item.ProjectCount = _context.Projects.Where(proj => proj.OwnerId == user.Id).Count();
                item.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";
            }

            return new ApiResult<UsersInfo>(
                data,
                pageIndex,
                pageSize,
                count,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
                );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            return Json(await _userManager.FindByIdAsync(id));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> BanUser(string id)
        {
            (await _userManager.FindByIdAsync(id)).LockoutEnd = DateTime.Now.AddYears(100);

            return Json(true);
        }
    }
}

