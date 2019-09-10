using Dapper;
using Polls.Core.Domain;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Repositories
{
    public class PollsRepository : IPollsRepository
    {
        private readonly PollsContext _context;
        
        public PollsRepository(PollsContext ctx)
        {
            _context = ctx;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Poll with given id and populated list of questions</returns>
        public async Task<Poll> Get(int id)
        {
            var sql = @"SELECT * FROM dbo.Polls p WHERE p.Id = @id;
                        SELECT * FROM dbo.SingleChoiceQuestions scq WHERE scq.PollId = @id;
                        SELECT * FROM dbo.TextAnswerQuestions taq WHERE taq.PollId = @id;
                        SELECT * FROM dbo.MultipleChoiceQuestions mcq WHERE mcq.PollId = @id";

            var reader = await _context.Conn.QueryMultipleAsync(sql, new { id }, transaction: _context.Transaction);
            var poll = reader.ReadSingle<Poll>();



            poll.AddQuestions(reader.Read<SingleChoiceQuestion>());
            poll.AddQuestions(reader.Read<TextAnswerQuestion>());
            poll.AddQuestions(reader.Read<MultipleChoiceQuestion>());

            return poll;
        }

        public async Task<IEnumerable<Poll>> GetAll(string userId)
        {
            var sql = "SELECT * FROM dbo.Polls p WHERE p.UserId = @userId";
            return await _context.Conn.QueryAsync<Poll>(sql, new { userId }, transaction: _context.Transaction);
        }
    }
}
