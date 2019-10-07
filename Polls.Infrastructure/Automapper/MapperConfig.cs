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
                .ReverseMap()
                .IncludeAllDerived();

            CreateMap<SingleChoiceQuestion, SingleChoiceQuestionDto>().ReverseMap();

            CreateMap<MultipleChoiceQuestion, MultipleChoiceQuestionDto>().ReverseMap();

            CreateMap<TextAnswerQuestion, TextAnswerQuestionDto>().ReverseMap();
        }
    }
}
