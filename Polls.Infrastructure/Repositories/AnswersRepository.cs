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
    }
}
