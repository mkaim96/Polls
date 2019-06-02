using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Mvc.Models.Polls
{
    public class DetailsViewModel
    {
        public int QuestionCount { get; set; }
        public PollDto Poll { get; set; }
    }
}
