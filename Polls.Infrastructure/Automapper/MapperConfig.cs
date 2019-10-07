using AutoMapper;
using Polls.Core.Domain;
using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Automapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Question, QuestionDto>()
                .IncludeAllDerived();

            CreateMap<QuestionDto, Question>()
                .IncludeAllDerived();

            CreateMap<SingleChoiceQuestion, SingleChoiceQuestionDto>();
            CreateMap<SingleChoiceQuestionDto, SingleChoiceQuestion>();

            CreateMap<MultipleChoiceQuestion, MultipleChoiceQuestionDto>();
            CreateMap<MultipleChoiceQuestionDto, MultipleChoiceQuestion>();

            CreateMap<TextAnswerQuestion, TextAnswerQuestionDto>();
            CreateMap<TextAnswerQuestionDto, TextAnswerQuestion>();
        }
    }
}
