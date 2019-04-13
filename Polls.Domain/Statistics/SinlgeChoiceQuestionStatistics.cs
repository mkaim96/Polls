using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Statistics
{
    public class SinlgeChoiceQuestionStatistics : QuestionStatistics
    {
        public int VotesCount { get; set; }
        public Dictionary<string, int> ChoicesCount { get; set; }
    }
}
