using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Dto
{
    public class MultipleChoiceQuestionDto : QuestionDto
    {
        public IEnumerable<string> Choices { get; set; }

    }
}
