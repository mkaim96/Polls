using Polls.Core.Domain;
using Polls.Core.Statistics;
using Polls.Core.Statistics.StatsGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Polls.Tests
{
    public class TextAnswerQuestionStatsGenerator
    {
        [Fact]
        public void wrong_question_type_should_throw_exception()
        {
            // Arrange.
            var generator = new TextAnswerQuestionStatisticsGenerator();
            var q = new SingleChoiceQuestion(1, "Lorem ipsum", 1, new List<string> { "a", "b", "c"});
            var answers = new List<Answer>();

            // Act.
            var e = Assert.Throws<ArgumentException>(() => generator.Generate(q, answers));
        }

        [Fact]
        public void all_passed_answers_should_be_included_in_stats()
        {
            // Arrange.
            var generator = new TextAnswerQuestionStatisticsGenerator();
            var questionId = Guid.NewGuid().ToString();
            var q = new TextAnswerQuestion(1, "Lorem ipsum", 1);

            var answers = new List<Answer> 
            {
                new TextAnswer { Answer = "asda dasdas", Id = 1, QuestionId = questionId },
                new TextAnswer { Answer = "asda dasdas 1", Id = 2, QuestionId = questionId },
                new TextAnswer { Answer = "asda dasdas 2", Id = 3, QuestionId = questionId },
                new TextAnswer { Answer = "asda dasdas 3", Id = 4, QuestionId = questionId }
            };

            // Act.
            var stats = (TextAnswerQuestionStatistics)generator.Generate(q, answers);


            // Assert.
            Assert.Equal(stats.Answers.ToList().Count, answers.Count);

        }
    }
}
