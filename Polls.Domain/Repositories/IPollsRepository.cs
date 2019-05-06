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
        Task Delete(int id);
    }
}
