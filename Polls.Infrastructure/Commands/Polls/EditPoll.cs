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

        public List<SingleChoiceQuestionDto> ScQuestionsToUpdate { get; set; }
        public List<TextAnswerQuestionDto> TaQuestionsToUpdate { get; set; }
        public List<MultipleChoiceQuestionDto> McQuestionsToUpdate { get; set; }

        public List<SingleChoiceQuestionDto> NewScQuestions { get; set; }
        public List<TextAnswerQuestionDto> NewTaQuestions { get; set; }
        public List<MultipleChoiceQuestionDto> NewMcQuestions { get; set; }

        public List<SingleChoiceQuestionDto> ScQuestionsToDelete { get; set; }
        public List<TextAnswerQuestionDto> TaQuestionsToDelete { get; set; }
        public List<MultipleChoiceQuestionDto> McQuestionsToDelete { get; set; }
    }
}
