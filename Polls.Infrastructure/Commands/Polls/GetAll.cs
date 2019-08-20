using MediatR;
using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class GetAll : IRequest<IEnumerable<PollDto>>
    {
        public string UserId { get; set; }
    }
}
