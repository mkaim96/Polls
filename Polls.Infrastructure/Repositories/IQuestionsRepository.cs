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

        Task<int> DeleteMany(IEnumerable<SingleChoiceQuestionDto> questions);
        Task<int> DeleteMany(IEnumerable<MultipleChoiceQuestionDto> questions);
        Task<int> DeleteMany(IEnumerable<TextAnswerQuestionDto> questions);

        Task<int> UpdateMany(IEnumerable<SingleChoiceQuestionDto> questions);
        Task<int> UpdateMany(IEnumerable<MultipleChoiceQuestionDto> questions);
        Task<int> UpdateMany(IEnumerable<TextAnswerQuestionDto> questions);
    }
}