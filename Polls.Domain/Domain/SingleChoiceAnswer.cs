using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Core.Domain
{
    public class SingleChoiceAnswer : Answer
    {
        public string Choice { get; set; }
    }
}
