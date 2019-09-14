using System.Collections.Generic;
using System.Threading.Tasks;
using Polls.Core.Domain;

namespace Polls.Infrastructure.Repositories
{
    public interface IPollsRepository
    {
        Task<IEnumerable<Poll>> GetAll(string userId);
        Task<int> Delete(int pollId);
        Task<Poll> Get(int id);
        Task<int> Add(Poll poll);
        Task<int> Update(int id, string newTitle, string newDescription);
    }
}