using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.Dto
{
    public class PollDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }
}
