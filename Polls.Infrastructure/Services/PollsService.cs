using Polls.Core.Domain;
using Polls.Core.Repositories;
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

                IPollsRepository pollsRepo = new PollsRepositoryTr(tr);

                await pollsRepo.Delete(id);

                tr.Commit();
            }
        }

        public async Task<PollDto> Get(int id)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();
                var tr = cnn.BeginTransaction();

                IPollsRepository pollsRepo = new PollsRepositoryTr(tr);

                var poll = await pollsRepo.Get(id);

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

                IPollsRepository pollsRepo = new PollsRepositoryTr(tr);

                var polls = await pollsRepo.GetAll(userId);

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
