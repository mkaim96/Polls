using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Statistics
{
    public class TextAnswerQuestionStatistics : QuestionStatistics
    {
        public int AnswersCount { get; set; }
        public IEnumerable<string> Answers { get; set; }
    }
}
