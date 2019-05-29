using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Core.Repositories
{
    public interface IPollsRepository
    {
        Task<Poll> Get(int id);
        Task<IEnumerable<Poll>> GetAll(string userId);
        Task<int> Delete(int id);
        Task<int> InsertPollOutId(Poll poll);
        Task<int> Update(Poll poll);
        Task<IEnumerable<Answer>> GetAnswers(int pollId);
    }
}
