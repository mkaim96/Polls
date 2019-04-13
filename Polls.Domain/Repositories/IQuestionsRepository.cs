using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Core.Repositories
{
    public interface IQuestionsRepository
    {
        Task<Dictionary<int, List<Answer>>> GetQuestionsWithAnswers(int pollId);
    }
}
