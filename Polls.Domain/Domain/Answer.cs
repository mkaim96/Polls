using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public abstract class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
    }
}
