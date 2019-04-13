using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Services.Interfaces;
using Polls.Mvc.Models;

namespace Polls.Mvc.Controllers
{
    public class HomeController : Controller
    {
        // For testing
        private string UserId = "f6ee4ba9-335e-4415-b86c-330c2efe9edf";


        private readonly IMediator _mediator;
        private IPollsService _pollsService;

        public HomeController(IMediator mediator, IPollsService pollsService)
        {
            _mediator = mediator;
            _pollsService = pollsService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _pollsService.GetAll(UserId);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
