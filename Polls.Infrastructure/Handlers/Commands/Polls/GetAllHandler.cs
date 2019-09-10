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
    public class GetAllHandler : IRequestHandler<GetAll, IEnumerable<PollDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<IEnumerable<PollDto>> Handle(GetAll request, CancellationToken cancellationToken)
        {

            var polls = await _unitOfWork.Polls.GetAll(request.UserId);
            _unitOfWork.Complete();

            return polls.Select(x => new PollDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            });
        }
    }
}
