using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Polls.Core.Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QuestionType
    {
        SingleChoice,
        TextAnswer,
        MultipleChoice
    }
}
