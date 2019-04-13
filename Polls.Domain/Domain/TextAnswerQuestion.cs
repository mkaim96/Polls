using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class TextAnswerQuestion : Question
    {
        public TextAnswerQuestion()
        {
            statsGenerator = new TextAnswerQuestionStatisticsGenerator();
        }
    }
}
