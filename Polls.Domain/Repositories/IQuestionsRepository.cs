using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Core.Repositories
{
    public interface IQuestionsRepository
    {
        Task<int> Insert(IEnumerable<SingleChoiceQuestion> questions);
        Task<int> Insert(IEnumerable<TextAnswerQuestion> questions);
        Task<int> Update(IEnumerable<SingleChoiceQuestion> questions);
        Task<int> Update(IEnumerable<TextAnswerQuestion> questions);

    }
}
