using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class DeletePoll : IRequest
    {
        public int Id { get; set; }
    }
} 