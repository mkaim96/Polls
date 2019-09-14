using MediatR;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Repositories;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class DeletePollHandler : AsyncRequestHandler<DeletePoll>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePollHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        protected override async Task Handle(DeletePoll request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Polls.Delete(request.Id);
            _unitOfWork.Complete();
        }
    }
}
