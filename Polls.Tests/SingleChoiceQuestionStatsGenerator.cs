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
        SingleChoiceQuestionStatisticsGenerator statsGenerator;
        SingleChoiceQuestion question;
        List<Answer> answers;

        public SingleChoiceQuestionStatsGenerator()
        {
            // Arrange.
            statsGenerator = new SingleChoiceQuestionStatisticsGenerator();
            question = new SingleChoiceQuestion(1, "test question", 1, new string[] { "opcja 1", "opcja 2", "opcja 3" });
            answers = new List<Answer>
            {
                new SingleChoiceAnswer {Id = 1, Choice = "opcja 1", QuestionId = question.Id },
                new SingleChoiceAnswer {Id = 2, Choice = "opcja 2", QuestionId = question.Id },
                new SingleChoiceAnswer {Id = 3, Choice = "opcja 3", QuestionId = question.Id },
                new SingleChoiceAnswer {Id = 4, Choice = "opcja 1", QuestionId = question.Id },

            };
        }

        [Fact]
        public void wrong_quetion_type_should_throw_exception()
        {
            // Arrange.
            var wrongQuestion = new TextAnswerQuestion(1, "test question", 1);

            // Act/Assert.
            Assert.Throws<ArgumentException>(() => statsGenerator.Generate(wrongQuestion, answers));
        }

        [Fact]
        public void should_count_answers()
        {
            // Arange.
            var expected = answers.Count;

            // Act.
            var stats = (SinlgeChoiceQuestionStatistics)statsGenerator.Generate(question, answers);

            // Assert.
            var actual = stats.VotesCount;
            Assert.Equal(expected, actual);

        }

        [Theory]
        [InlineData("opcja 1", 2)]
        [InlineData("opcja 2", 1)]
        [InlineData("opcja 3", 1)]
        public void should_count_votes(string choice, int votes)
        {
            // Act.
            var stats = (SinlgeChoiceQuestionStatistics)statsGenerator.Generate(question, answers);

            // Assert.
            var actual = stats.ChoicesCount[choice];
            Assert.Equal(actual, votes);

            
        }
    }
}
