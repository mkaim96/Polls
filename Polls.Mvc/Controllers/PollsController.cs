using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polls.Core.Repositories;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Dto;
using Polls.Infrastructure.Ef;
using Polls.Infrastructure.Services.Interfaces;
using Polls.Mvc.Models;
using Polls.Mvc.Models.Polls;

namespace Polls.Mvc.Controllers
{
    [Route("polls/")]
    public class PollsController : Controller
    {
        private IPollsService _pollsService;
        private UserManager<ApplicationUser> _userManager;
        private IMediator _mediator;

        public PollsController(IMediator mediator, IPollsService pollsService, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _pollsService = pollsService;
            _userManager = userManager;
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
            var poll = await _pollsService.Get(id);
            var model = new DetailsViewModel
            {
                Poll = poll,
                QuestionCount = poll.Questions.Count()
            };
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
            request.UserId = GetUserId();

            await _mediator.Send(request);

            return Ok();
        }

        [Route("create")]
        public IActionResult CreatePoll()
        {
            return View();
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

        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pollsService.Delete(id);
            return RedirectToAction("Index", "Home");
        }

        [Route("edit")]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody]EditPoll request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("edit/poll/{id}")]
        public async Task<IActionResult> GetPollToEdit(int id)
        {
            var poll =  await _pollsService.Get(id);

            return Ok(new {
                pollId = poll.Id,
                title = poll.Title,
                description = poll.Description,
                questions = poll.Questions.OrderBy(x => x.Number)
            });;
        }

        #region helpers
        private string GetUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }
        #endregion
    }
}