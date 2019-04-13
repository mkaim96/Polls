using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Statistics
{
    public interface IStatisticsGenerator
    {
        QuestionStatistics Generate(Question q, List<Answer> answers);
    }
}
