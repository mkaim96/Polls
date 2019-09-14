using Dapper;
using MediatR;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class ClearAnswersHandler : AsyncRequestHandler<ClearAnswers>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClearAnswersHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        protected override async Task Handle(ClearAnswers request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Answers.Clear(request.PollId);
            _unitOfWork.Complete();
        }
    }
}
