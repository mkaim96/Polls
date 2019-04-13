using MediatR;
using Polls.Core.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class GenerateStatistics : IRequest<IEnumerable<QuestionStatistics>>
    {
        public int PollId { get; set; }
    }
}
