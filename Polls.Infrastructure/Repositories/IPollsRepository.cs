using System.Collections.Generic;
using System.Threading.Tasks;
using Polls.Core.Domain;

namespace Polls.Infrastructure.Repositories
{
    public interface IPollsRepository
    {
        Task<IEnumerable<Poll>> GetAll(string userId);
        Task<Poll> Get(int id);
    }
}