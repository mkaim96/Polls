using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Polls.Infrastructure.Dapper.TypeHandlers
{
    public class CsvTypeHandler : SqlMapper.TypeHandler<IEnumerable<string>>
    {
        public override IEnumerable<string> Parse(object value)
        {
            var val = (string)value;
            return val.Split(",");
        }

        public override void SetValue(IDbDataParameter parameter, IEnumerable<string> value)
        {
            parameter.Value = string.Join(",", value);
        }
    }
}
