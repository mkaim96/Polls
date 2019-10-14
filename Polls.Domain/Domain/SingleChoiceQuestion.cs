using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class SingleChoiceQuestion : Question
    {
        public IEnumerable<string> Choices { get; set; }

        protected SingleChoiceQuestion()
        {
            statsGenerator = new SingleChoiceQuestionStatisticsGenerator();
        }

        public SingleChoiceQuestion(int pollId, string qText, int number, IEnumerable<string> choices) 
            : base(pollId, qText, number)
        {
            Choices = choices;
            QuestionType = QuestionType.SingleChoice;
            statsGenerator = new SingleChoiceQuestionStatisticsGenerator();
        }
        


    }
}
