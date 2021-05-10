using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CUAFunding.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : Controller
    {
        public Task<IActionResult> AddMessage(AddMessageViewModel viewModel)
        {

        }

        public Task<IActionResult> RedactMessage(UpdateMessageViewModel viewModel)
        {

        }

        public Task<IActionResult> ChangeUserStatus(ChangeUserStatusViewModel viewModel)
        {

        }

        public Task<IActionResult> BanUser(BanUserViewModel viewModel)
        {

        }

        public Task<IActionResult> LoadMessages(GetMessagesViewModel viewModel)
        {

        }
    }
}
