using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Ef;
using Polls.Mvc.Models;

namespace Polls.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var request = new GetAll { UserId = GetUserId() };
            var model = await _mediator.Send(request);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Helpers

        private string GetUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }

        #endregion
    }
}
