using Polls.Core.Repositories;
using Polls.Infrastructure.Dto;
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
        private IPollsRepository _pollsRepository;

        public PollsService(IPollsRepository pollsRepo)
        {
            _pollsRepository = pollsRepo;
        }

        public async Task<PollDto> Get(int id)
        {
            var poll = await _pollsRepository.Get(id);

            return new PollDto
            {
                Id = poll.Id,
                Questions = poll.Questions,
                Description = poll.Description,
                Title = poll.Title
            };
        }

        public async Task<IEnumerable<PollDto>> GetAll(string userId)
        {
            var polls = await _pollsRepository.GetAll(userId);

            return polls.Select(x => new PollDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            });
        }
    }
}
