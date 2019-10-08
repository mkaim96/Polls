using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class TextAnswerQuestion : Question
    {
        public  TextAnswerQuestion(int pollId, QuestionType qType, string qText, int number) 
            : base(pollId, qType, qText, number)
        {
            statsGenerator = new TextAnswerQuestionStatisticsGenerator();
        }
        protected TextAnswerQuestion()
        {
            statsGenerator = new TextAnswerQuestionStatisticsGenerator();
        }
    }
}
