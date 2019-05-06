using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Services.Interfaces
{
    public interface IPollsService
    {
        Task<IEnumerable<PollDto>> GetAll(string userId);
        Task<PollDto> Get(int id);
        Task Delete(int id);
    }
}
