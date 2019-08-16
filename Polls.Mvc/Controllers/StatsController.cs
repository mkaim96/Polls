using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Polls.Infrastructure.Commands.Polls;

namespace Polls.Mvc.Controllers
{
    [Route("stats/")]
    public class StatsController : Controller
    {
        private IMediator _mediator;

        public StatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("charts/poll/{id}")]
        public IActionResult Charts()
        {
            return View();
        }

        [Route("poll/{id}")]
        public async Task<IActionResult> GetStatsJson(int id)
        {
            var request = new GenerateStatistics { PollId = id };
            var stat = await _mediator.Send(request);

            return Ok(stat);
        }
    }
}