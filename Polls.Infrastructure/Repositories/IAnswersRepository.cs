using System.Collections.Generic;
using System.Threading.Tasks;
using Polls.Core.Domain;

namespace Polls.Infrastructure.Repositories
{
    public interface IAnswersRepository
    {
        Task<IEnumerable<Answer>> GetAll(int pollId);
        Task<int> InsertMany(IEnumerable<SingleChoiceAnswer> answers);
        Task<int> InsertMany(IEnumerable<MultipleChoiceAnswer> answers);
        Task<int> InsertMany(IEnumerable<TextAnswer> answers);
        Task Clear(int pollId);
    }
}