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
    public class GetAllHandler : IRequestHandler<GetAll, IEnumerable<PollDto>>
    {
        public async Task<IEnumerable<PollDto>> Handle(GetAll request, CancellationToken cancellationToken)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                var tr = cnn.BeginTransaction();
                var repo = new QueriesRepoUoW(tr);

                var polls = await repo.GetAll(request.UserId);

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

                return polls.Select(x => new PollDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description
                });
            }
        }
    }
}
