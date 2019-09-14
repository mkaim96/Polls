using Polls.Core.Domain;
using Polls.Infrastructure.Dto;
using Polls.Infrastructure.Repositories;
using Polls.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Services
{
    public class PollsService : IPollsService
    {
        public async Task Delete(int id)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();
                var tr = cnn.BeginTransaction();

                var repo = new QueriesRepoUoW(tr);

                await repo.Delete(id);

                tr.Commit();
            }
        }

        public async Task<PollDto> Get(int id)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();
                var tr = cnn.BeginTransaction();

                var repo = new QueriesRepoUoW(tr);

                var poll = await repo.Get(id);

                tr.Commit();

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

        public async Task<IEnumerable<PollDto>> GetAll(string userId)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                var tr = cnn.BeginTransaction();

                var repo = new QueriesRepoUoW(tr);

                var polls = await repo.GetAll(userId);

                tr.Commit();
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
