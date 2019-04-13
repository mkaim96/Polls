using MediatR;
using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class CreatePoll : IRequest
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<SingleChoiceQuestionDto> SingleChoiceQuestions { get; set; }
        public IEnumerable<TextAnswerQuestionDto> TextAnswerQuestions { get; set; }
    }
}
