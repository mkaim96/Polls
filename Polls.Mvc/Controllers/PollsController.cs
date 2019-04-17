using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polls.Core.Repositories;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Services.Interfaces;
using Polls.Mvc.Models;

namespace Polls.Mvc.Controllers
{
    [Route("polls/")]
    public class PollsController : Controller
    {
        private IPollsService _pollsService;
        private IMediator _mediator;

        public PollsController(IMediator mediator, IPollsService pollsService)
        {
            _mediator = mediator;
            _pollsService = pollsService;
        }
        [Route("form/{id}")]
        public async Task<IActionResult> DisplayForm([FromRoute]int id)
        {
            var model = await _pollsService.Get(id);
            return View("Form", model);
        }

        [Route("details/{id}")]
        public async Task<IActionResult> Details([FromRoute]int id)
        {
            var model = await _pollsService.Get(id);
            return View(model);
        }

        [Route("{id}/statistics")]
        public async Task<IActionResult> Statistics(int id)
        {
            var request = new GenerateStatistics { PollId = id };
            var model = await _mediator.Send(request);

            return View(model);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePoll([FromBody]CreatePoll request)
        {
            await _mediator.Send(request);
            return RedirectToAction("Index", "Home");
        }

        [Route("submit-solution")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitSolution(IFormCollection form)
        {
            var request = new AddAnswers { Form = (FormCollection)form };
            _mediator.Send(request);

            // TODO: Return view with thanks to respondent
            return RedirectToAction("Index", "Home");
        }
    }
}