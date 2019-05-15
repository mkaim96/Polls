using MediatR;
using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class EditPoll : IRequest
    {
        public int PollId { get; set; }
        public string NewTitle { get; set; }
        public string NewDescription { get; set; }

        public IEnumerable<SingleChoiceQuestionDto> ScQuestionsToUpdate { get; set; }
        public IEnumerable<TextAnswerQuestionDto> TaQuestionsToUpdate { get; set; }

        public IEnumerable<SingleChoiceQuestionDto> NewScQuestions { get; set; }
        public IEnumerable<TextAnswerQuestionDto> NewTaQuestions { get; set; }

        public IEnumerable<SingleChoiceQuestionDto> ScQuestionsToDelete { get; set; }
        public IEnumerable<TextAnswerQuestionDto> TaQuestionsToDelete { get; set; }
    }
}
