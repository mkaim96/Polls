using MediatR;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Dto;
using Polls.Infrastructure.Repositories;
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
        public async Task<PollDto> Handle(GetPoll request, CancellationToken cancellationToken)
        {
            using(var cnn = Connection.GetConnection())
            {
                cnn.Open();

                var tr = cnn.BeginTransaction();
                var repo = new QueriesRepoUoW(tr);

                var poll = await repo.Get(request.Id);

                try
                {
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception type: {0}", ex.GetType());
                    Console.WriteLine(" Message: {0}", ex.Message);
                }

                cnn.Close();

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
}
