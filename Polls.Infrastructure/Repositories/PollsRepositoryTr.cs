using Dapper;
using Polls.Core.Domain;
using Polls.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Repositories
{
    public class PollsRepositoryTr : IPollsRepository
    {
        IDbTransaction tr;
        IDbConnection cnn => tr.Connection;

        public PollsRepositoryTr(IDbTransaction transaction)
        {
            tr = transaction;
        }
        public async Task<int> Delete(int id)
        {
            var sql = @"delete from dbo.SingleChoiceQuestions Where PollId = @id;
                        delete from dbo.TextAnswerQuestions Where PollId = @id;
                        delete from dbo.Polls Where Id = @id";

            return await cnn.ExecuteAsync(sql, new { id }, transaction: tr);
        }

        public async Task<Poll> Get(int id)
        {
            var sql = @"SELECT * FROM dbo.Polls p WHERE p.Id = @id;
                        SELECT * FROM dbo.SingleChoiceQuestions scq WHERE scq.PollId = @id;
                        SELECT * FROM dbo.TextAnswerQuestions taq WHERE taq.PollId = @id";

            var reader = await cnn.QueryMultipleAsync(sql, new { id }, transaction: tr);
            var poll = reader.ReadSingle<Poll>();


            poll.AddQuestions(reader.Read<SingleChoiceQuestion>());
            poll.AddQuestions(reader.Read<TextAnswerQuestion>());
            poll.Questions.OrderBy(x => x.Number);

            return poll;
        }

        public async Task<IEnumerable<Poll>> GetAll(string userId)
        {
            var sql = "SELECT * FROM dbo.Polls p WHERE p.UserId = @userId";
            return await cnn.QueryAsync<Poll>(sql, new { userId }, transaction: tr);
        }

        public async Task<int> InsertPollOutId(Poll poll)
        {
            var parameters = new { poll.UserId, poll.Title, poll.Description };

            return await cnn.QuerySingleAsync<int>("dbo.spPolls_Insert",
                parameters,
                transaction: tr,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Update(Poll poll)
        {
            var parameters = new
            {
                NewTitle = poll.Title,
                NewDescription =  poll.Description,
                PollId = poll.Id
            };

            return await cnn.ExecuteAsync("dbo.spPolls_Update",
                parameters,
                transaction: tr,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Answer>> GetAnswers(int pollId)
        {
            var sql = @"SELECT ta.Id, ta.QuestionId, ta.Answer
                                    FROM TextAnswers ta
                                    JOIN TextAnswerQuestions taQ on ta.QuestionId = taQ.Id
                                    WHERE taQ.PollId = @pollId;
                               SELECT scA.Id, scA.QuestionId, scA.Choice FROM SingleChoiceAnswers scA
                                    JOIN SingleChoiceQuestions scQ on scQ.Id = scA.QuestionId
                                    WHERE scQ.PollId = @pollId";


            var answersReader = await cnn.QueryMultipleAsync(sql, new { pollId }, transaction: tr);

            var answers = new List<Answer>();

            answers.AddRange(answersReader.Read<TextAnswer>());
            answers.AddRange(answersReader.Read<SingleChoiceAnswer>());

            return answers;
        }
    }
}
