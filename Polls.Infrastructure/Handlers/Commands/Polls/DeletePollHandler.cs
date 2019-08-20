using MediatR;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class DeletePollHandler : AsyncRequestHandler<DeletePoll>
    {
        protected override async Task Handle(DeletePoll request, CancellationToken cancellationToken)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                var tr = cnn.BeginTransaction();

                var repo = new QueriesRepoUoW(tr);

                await repo.Delete(request.Id);

                try
                {
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception type: {0}", ex.GetType());
                    Console.WriteLine(" Message: {0}", ex.Message);
                }
            }
        }
    }
}
