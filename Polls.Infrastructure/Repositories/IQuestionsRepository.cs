using System.Collections.Generic;
using System.Threading.Tasks;
using Polls.Core.Domain;
using Polls.Infrastructure.Dto;

namespace Polls.Infrastructure.Repositories
{
    public interface IQuestionsRepository
    {
        Task<int> InsertMany(IEnumerable<SingleChoiceQuestion> questions);
        Task<int> InsertMany(IEnumerable<MultipleChoiceQuestion> questions);
        Task<int> InsertMany(IEnumerable<TextAnswerQuestion> questions);

        Task<int> DeleteMany(IEnumerable<SingleChoiceQuestion> questions);
        Task<int> DeleteMany(IEnumerable<MultipleChoiceQuestion> questions);
        Task<int> DeleteMany(IEnumerable<TextAnswerQuestion> questions);

        Task<int> UpdateMany(IEnumerable<SingleChoiceQuestion> questions);
        Task<int> UpdateMany(IEnumerable<MultipleChoiceQuestion> questions);
        Task<int> UpdateMany(IEnumerable<TextAnswerQuestion> questions);
    }
}