using Polls.Core.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public abstract class Question
    {
        protected IStatisticsGenerator statsGenerator;
        public string Id { get; set; }
        public int PollId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Number { get; set; }

        protected Question()
        {

        }

        public Question(int pollId, QuestionType qType, string qText, int number)
        {
            Id = Guid.NewGuid().ToString();
            PollId = pollId;
            QuestionType = qType;
            QuestionText = qText;
            Number = number;
        }

        public QuestionStatistics GenerateStatistics(List<Answer> answers)
        {
            return statsGenerator.Generate(this, answers);
        }
    }
}

