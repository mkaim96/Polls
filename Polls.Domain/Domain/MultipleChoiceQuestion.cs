using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class MultipleChoiceQuestion : Question
    {
        public IEnumerable<string> Choices { get; private set; }

        protected MultipleChoiceQuestion()
        {
            statsGenerator = new MultipleChoiceQuestionStatisticsGenerator();
        }
        public MultipleChoiceQuestion(int pollId, string qText, int number, IEnumerable<string> choices)
            : base(pollId, qText, number)
        {
            statsGenerator = new MultipleChoiceQuestionStatisticsGenerator();
            QuestionType = QuestionType.MultipleChoice;
            Choices = choices;
        }
    }
}
