using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polls.Infrastructure.Commands.Polls;

namespace Polls.Mvc.Controllers
{
    public class AnswersController : Controller
    {
        private IMediator _mediator;

        public AnswersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAnswers(IFormCollection form)
        {
            var request = new AddAnswers { Form = (FormCollection)form };
            await _mediator.Send(request);

            //var pollId = Convert.ToInt32(form["PollId"]);

            // TODO: Return view with thanks to respondent
            return RedirectToAction("Index", "Home");
        }

        [Route("clear/{id}")]
        public async Task<IActionResult> Clear(int id)
        {
            var request = new ClearAnswers { PollId = id };
            await _mediator.Send(request);

            return RedirectToAction("Details", "Polls", new { id });
        }
    }
}