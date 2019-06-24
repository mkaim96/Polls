using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class MultipleChoiceAnswer : Answer
    {
        public IEnumerable<string> Choices { get; set; }
    }
}
