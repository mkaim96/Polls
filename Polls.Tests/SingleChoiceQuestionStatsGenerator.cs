using Polls.Core.Domain;
using Polls.Core.Statistics;
using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Polls.Tests
{
    public class SingleChoiceQuestionStatsGenerator
    {
        [Theory]
        [InlineData("opcja 1", 2)]
        [InlineData("opcja 2", 1)]
        [InlineData("opcja 3", 1)]
        public void should_count_votes(string choice, int votes)
        {
            // Arange.
            var statGenerator = new SingleChoiceQuestionStatisticsGenerator();
            var question = new SingleChoiceQuestion(1, "test question", 1, new string[] { "opcja 1", "opcja 2", "opcja 3" });
            var answers = new List<Answer>
            {
                new SingleChoiceAnswer {Id = 1, Choice = "opcja 1", QuestionId = question.Id },
                new SingleChoiceAnswer {Id = 2, Choice = "opcja 2", QuestionId = question.Id },
                new SingleChoiceAnswer {Id = 3, Choice = "opcja 3", QuestionId = question.Id },
                new SingleChoiceAnswer {Id = 4, Choice = "opcja 1", QuestionId = question.Id },

            };

            // Act.
            var stats = (SinlgeChoiceQuestionStatistics)statGenerator.Generate(question, answers);

            // Assert.
            var actual = stats.ChoicesCount[choice];
            Assert.Equal(actual, votes);

            
        }
    }
}
