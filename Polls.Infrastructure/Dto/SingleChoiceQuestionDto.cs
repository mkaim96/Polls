using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Dto
{
    public class SingleChoiceQuestionDto : QuestionDto
    {
        public IEnumerable<string> Choices { get; set; }
    }
}
