using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class SingleChoiceQuestion : Question
    {
        public IEnumerable<string> Choices { get; set; }

        public SingleChoiceQuestion()
        {
            statsGenerator = new SingleChoiceQuestionStatisticsGenerator();
        }

        public SingleChoiceQuestion(int pollId, QuestionType qType, string qText, int number, IEnumerable<string> choices) 
            : base(pollId, qType, qText, number)
        {
            Choices = choices;
            statsGenerator = new SingleChoiceQuestionStatisticsGenerator();
        }
        


    }
}
