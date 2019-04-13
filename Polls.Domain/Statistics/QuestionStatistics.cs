using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Statistics
{
    public abstract class QuestionStatistics
    {
        public Question Question { get; set; }
    }
}
