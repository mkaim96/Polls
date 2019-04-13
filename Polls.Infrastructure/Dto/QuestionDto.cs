using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Dto
{
    public abstract class QuestionDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Number { get; set; }
    }
}
