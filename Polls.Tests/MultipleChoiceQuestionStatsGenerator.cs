using Polls.Core.Domain;
using Polls.Core.Statistics;
using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Polls.Tests
{
    public class MultipleChoiceQuestionStatsGenerator
    {
        MultipleChoiceQuestionStatisticsGenerator statsGenerator;
        MultipleChoiceQuestion question;
        List<Answer> answers;

        public MultipleChoiceQuestionStatsGenerator()
        {
            statsGenerator = new MultipleChoiceQuestionStatisticsGenerator();
            question = new MultipleChoiceQuestion(1, "test question", 1, new string[] { "opcja 1", "opcja 2", "opcja 3"});
            answers = new List<Answer>
            {
                new MultipleChoiceAnswer { Choices = new string[] {"opcja 1", "opcja 3"}, Id = 1, QuestionId = question.Id },
                new MultipleChoiceAnswer { Choices = new string[] {"opcja 1"}, Id = 2, QuestionId = question.Id },
                new MultipleChoiceAnswer { Choices = new string[] {"opcja 2", "opcja 3"}, Id = 3, QuestionId = question.Id },
                new MultipleChoiceAnswer { Choices = new string[] {"opcja 1", "opcja 2"}, Id = 4, QuestionId = question.Id }
            };
        }

        [Theory]
        [InlineData("opcja 1" , 3)]
        [InlineData("opcja 2", 2)]
        [InlineData("opcja 3", 2)]
        public void should_count_votes(string choice, int expected)
        {
            // Act.
            var stats = (MultipleChoiceQuestionStatistics)statsGenerator.Generate(question, answers);

            // Assert.
            var actual = stats.ChoicesCount[choice];
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void should_count_answers()
        {
            // Arrange.
            var expected = answers.Count;
            
            // Act.
            var stats = (MultipleChoiceQuestionStatistics)statsGenerator.Generate(question, answers);

            // Assert.
            var actual = stats.VotesCount;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void wrong_question_type_should_throw_exception()
        {
            // Arange.
            var wrongQuestion = new TextAnswerQuestion(1, "test", 1);

            // Act/Assert.
            Assert.Throws<ArgumentException>(() => statsGenerator.Generate(wrongQuestion, answers));
        }
    }
}
