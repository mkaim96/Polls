using Polls.Core.Domain;
using Polls.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Polls.Infrastructure.Dapper.TypeHandlers;
using System.Linq;

namespace Polls.Infrastructure.Repositories
{
    public class PollsRepository : IPollsRepository
    {
        public async Task<Poll> Get(int id)
        {
            var sql = @"SELECT * FROM dbo.Polls p WHERE p.Id = @id;
                        SELECT * FROM dbo.SingleChoiceQuestions scq WHERE scq.PollId = @id;
                        SELECT * FROM dbo.TextAnswerQuestions taq WHERE taq.PollId = @id";

            using (var cnn = Connection.GetConnection())
            {
                var reader = await cnn.QueryMultipleAsync(sql, new { id }); 
                var poll = reader.ReadSingle<Poll>();
                
              
                poll.AddQuestions(reader.Read<SingleChoiceQuestion>());
                poll.AddQuestions(reader.Read<TextAnswerQuestion>());

                cnn.Close();

                return poll;
            }   
        }

        public async Task<IEnumerable<Poll>> GetAll(string userId)
        {
            var sql = "SELECT * FROM dbo.Polls p WHERE p.UserId = @userId";

            using (var cnn = Connection.GetConnection())
            {
                var result = await cnn.QueryAsync<Poll>(sql, new { userId });
                return result;
            }
        }

        public async Task Delete(int id)
        {
            var sql = @"delete from dbo.SingleChoiceQuestions where PollId = @id;
                        delete from dbo.TextAnswerQuestions where PollId = @id;
                        delete from dbo.Polls where Id = @id";

            using (var cnn = Connection.GetConnection())
            {
                await cnn.ExecuteAsync(sql, new { id });
            }
        }
    }
}
