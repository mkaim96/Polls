using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Commands.Polls
{
    public class AddAnswers : IRequest
    {
        public FormCollection Form { get; set; }
    }
}
