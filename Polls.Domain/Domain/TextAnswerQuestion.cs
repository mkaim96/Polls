using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class TextAnswerQuestion : Question
    {
        public  TextAnswerQuestion(int pollId, string qText, int number) 
            : base(pollId, qText, number)
        {
            statsGenerator = new TextAnswerQuestionStatisticsGenerator();
            QuestionType = QuestionType.TextAnswer;
        }
        protected TextAnswerQuestion()
        {
            statsGenerator = new TextAnswerQuestionStatisticsGenerator();
        }
    }
}
