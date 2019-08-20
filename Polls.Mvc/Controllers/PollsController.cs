using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Ef;
using Polls.Infrastructure.Services.Interfaces;
using Polls.Mvc.Models.Polls;

namespace Polls.Mvc.Controllers
{
    [Route("polls/")]
    public class PollsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IMediator _mediator;

        public PollsController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }


        [Route("form/{id}")]
        public async Task<IActionResult> DisplayForm([FromRoute]int id)
        {
            var request = new GetPoll { Id = id };
            var model = await _mediator.Send(request);

            return View("Form", model);
        }

        [Route("details/{id}")]
        public async Task<IActionResult> Details([FromRoute]int id)
        {
            var request = new GetPoll { Id = id };
            var poll = await _mediator.Send(request);

            var model = new DetailsViewModel
            {
                Poll = poll,
                QuestionCount = poll.Questions.Count()
            };

            return View(model);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody]CreatePoll request)
        {
            request.UserId = GetUserId();

            await _mediator.Send(request);

            return Ok();
        }

        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        { 

            var request = new DeletePoll { Id = id };
            await _mediator.Send(request);

            return RedirectToAction("Index", "Home");
        }

        [Route("edit")]
        [HttpGet]
        public IActionResult Edit()
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
            var request = new GetPoll { Id = id };
            var poll = await _mediator.Send(request);

            return Ok(new
            {
                pollId = poll.Id,
                title = poll.Title,
                description = poll.Description,
                questions = poll.Questions.OrderBy(x => x.Number)
            }); ;
        }

        #region helpers
        private string GetUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }
        #endregion
    }
}