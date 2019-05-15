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
        public List<SingleChoiceQuestionDto> SingleChoiceQuestions { get; set; }
        public List<TextAnswerQuestionDto> TextAnswerQuestions { get; set; }
    }
}
