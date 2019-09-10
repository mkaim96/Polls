using System.Collections.Generic;
using System.Threading.Tasks;
using Polls.Core.Domain;

namespace Polls.Infrastructure.Repositories
{
    public interface IAnswersRepository
    {
        Task<IEnumerable<Answer>> GetAll(int pollId);
    }
}