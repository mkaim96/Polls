using Dapper;
using Polls.Core.Domain;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Repositories
{
    public class AnswersRepository : IAnswersRepository
    {
        private readonly PollsContext _context;

        public AnswersRepository(PollsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes all answers for poll with given id
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns></returns>
        public async Task Clear(int pollId)
        {
            var deleteScaSql = @"DELETE sca 
                        FROM dbo.SingleChoiceAnswers sca 
                        JOIN dbo.SingleChoiceQuestions scq on sca.QuestionId = scq.Id
                        JOIN dbo.Polls p on scq.PollId = p.Id
                        WHERE p.Id = @PollId";

            var deleteMcaSql = @"DELETE mca
                        FROM dbo.MultipleChoiceAnswers mca
                        JOIN dbo.MultipleChoiceQuestions mcq on mca.QuestionId = mcq.Id
                        JOIN dbo.Polls p on mcq.PollId = p.Id";

            var deleteTaSql = @"DELETE ta
                        FROM dbo.TextAnswers ta
                        JOIN dbo.TextAnswerQuestions taq on ta.QuestionId = taq.Id
                        JOIN dbo.Polls p on taq.PollId = p.Id";


            var t1 = _context.Conn.ExecuteAsync(deleteScaSql, new { PollId = pollId}, transaction: _context.Transaction);
            var t2 = _context.Conn.ExecuteAsync(deleteMcaSql, new { PollId = pollId }, transaction: _context.Transaction);
            var t3 = _context.Conn.ExecuteAsync(deleteTaSql, new { PollId = pollId}, transaction: _context.Transaction);

            await Task.WhenAll(t1, t2, t3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns>All answers for poll with given id</returns>
        public async Task<IEnumerable<Answer>> GetAll(int pollId)
        {
            var sql = @"SELECT ta.Id, ta.QuestionId, ta.Answer
                                    FROM TextAnswers ta
                                    JOIN TextAnswerQuestions taQ on ta.QuestionId = taQ.Id
                                    WHERE taQ.PollId = @pollId;
                               SELECT scA.Id, scA.QuestionId, scA.Choice FROM SingleChoiceAnswers scA
                                    JOIN SingleChoiceQuestions scQ on scQ.Id = scA.QuestionId
                                    WHERE scQ.PollId = @pollId;
                               SELECT mcA.Id, mcA.QuestionId, mcA.Choices FROM MultipleChoiceAnswers mcA
                                    JOIN MultipleChoiceQuestions mcQ on mcQ.Id = mcA.QuestionId
                                    WHERE mcQ.PollId = @pollId";


            var answersReader = await _context.Conn.QueryMultipleAsync(sql, new { pollId }, transaction: _context.Transaction);

            var answers = new List<Answer>();

            answers.AddRange(answersReader.Read<TextAnswer>());
            answers.AddRange(answersReader.Read<SingleChoiceAnswer>());
            answers.AddRange(answersReader.Read<MultipleChoiceAnswer>());

            return answers;
        }

        public async Task<int> InsertMany(IEnumerable<SingleChoiceAnswer> answers)
        {
            var sql = @"INSERT INTO dbo.SingleChoiceAnswers (QuestionId, Choice) VALUES (@QuestionId, @Choice)";

            return await _context.Conn.ExecuteAsync(sql, answers, transaction: _context.Transaction);

        }

        public async Task<int> InsertMany(IEnumerable<MultipleChoiceAnswer> answers)
        {
            var sql = @"INSERT INTO dbo.MultipleChoiceAnswers (QuestionId, Choices) VALUES (@QuestionId, @Choices)";

            return await _context.Conn.ExecuteAsync(sql, answers, transaction: _context.Transaction);
        }

        public async Task<int> InsertMany(IEnumerable<TextAnswer> answers)
        {
            var sql = @"INSERT INTO dbo.TextAnswers (QuestionId, Answer) VALUES (@QuestionId, @Answer)";

            return await _context.Conn.ExecuteAsync(sql, answers, transaction: _context.Transaction);
        }
    }
}
