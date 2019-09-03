using System;
using System.Collections.Generic;
using System.Text;
using Polls.Core.Domain;

namespace Polls.Core.Statistics.StatsGenerators
{
    public class TextAnswerQuestionStatisticsGenerator : IStatisticsGenerator
    {
        public QuestionStatistics Generate(Question q, List<Answer> answers)
        {
            if (!(q is TextAnswerQuestion))
            {
                throw new ArgumentException();
            }

            var stats = new TextAnswerQuestionStatistics
            {
                Question = q,
                AnswersCount = answers.Count
            };

            var answrs = new List<string>();
            foreach (TextAnswer answer in answers)
            {
                answrs.Add(answer.Answer);
            }

            stats.Answers = answrs;

            return stats;

        }
    }
}
