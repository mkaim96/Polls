using System;
using System.Collections.Generic;
using System.Text;
using Polls.Core.Domain;

namespace Polls.Core.Statistics.StatsGenerators
{
    public class SingleChoiceQuestionStatisticsGenerator : IStatisticsGenerator
    {
        public QuestionStatistics Generate(Question q, List<Answer> answers)
        {
            var question = (SingleChoiceQuestion)q;

            var stats = new SinlgeChoiceQuestionStatistics
            {
                Question = question,
                VotesCount = answers.Count,
            };

            var choicesCount = new Dictionary<string, int>();

            // Initialize dictionary with each choice as key and give it an initial value of 0.
            foreach (var choice in question.Choices)
            {
                choicesCount.Add(choice, 0);
            }

            // Go through every answer and increase count.
            foreach(SingleChoiceAnswer answer in answers)
            {
                if(choicesCount.ContainsKey(answer.Choice))
                {
                    choicesCount[answer.Choice] += 1;
                }
            }

            stats.ChoicesCount = choicesCount;

            return stats;
        }
    }
}
