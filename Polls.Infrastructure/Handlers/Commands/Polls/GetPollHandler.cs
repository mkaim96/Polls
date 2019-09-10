using MediatR;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Dto;
using Polls.Infrastructure.Repositories;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    class GetPollHandler : IRequestHandler<GetPoll, PollDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPollHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<PollDto> Handle(GetPoll request, CancellationToken cancellationToken)
        {
            var poll = await _unitOfWork.Polls.Get(request.Id);
            _unitOfWork.Complete();

            return new PollDto
            {
                Id = poll.Id,
                Questions = poll.Questions.OrderBy(x => x.Number),
                Description = poll.Description,
                Title = poll.Title
            };
        }
    }
}
