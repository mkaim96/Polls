using System;
using System.Collections.Generic;
using System.Text;
using Polls.Core.Domain;

namespace Polls.Core.Statistics.StatsGenerators
{
    class MultipleChoiceQuestionStatisticsGenerator : IStatisticsGenerator
    {
        public QuestionStatistics Generate(Question q, List<Answer> answers)
        {
            if (!(q is MultipleChoiceQuestion))
            {
                throw new ArgumentException();
            }

            var question = (MultipleChoiceQuestion)q;

            var choicesCount = new Dictionary<string, int>();

            // Initialize dictionary with each choice as key and give it an initial value of 0.
            foreach (var choice in question.Choices)
            {
                choicesCount.Add(choice, 0);
            }

            foreach(MultipleChoiceAnswer answer in answers)
            {
                foreach(var choice in answer.Choices)
                {
                    if(choicesCount.ContainsKey(choice))
                    {
                        choicesCount[choice] += 1;
                    }
                }
            }

            var stats = new MultipleChoiceQuestionStatistics
            {
                Question = question,
                VotesCount = answers.Count,
                ChoicesCount = choicesCount

            };

            return stats;
        }
    }
}
