using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class ClearAnswers : IRequest
    {
        public int PollId { get; set; }
    }
}
