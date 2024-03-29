﻿using Polls.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Polls.Infrastructure.Dto
{
    public abstract class QuestionDto
    {
        public string Id { get; set; }
        public int PollId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public int Number { get; set; }
    }
}
