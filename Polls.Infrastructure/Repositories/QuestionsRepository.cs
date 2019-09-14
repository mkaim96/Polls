using Dapper;
using Polls.Core.Domain;
using Polls.Infrastructure.Dto;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Repositories
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly PollsContext _context;

        public QuestionsRepository(PollsContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteMany(IEnumerable<SingleChoiceQuestionDto> questions)
        {
            var sql = @"delete from dbo.SingleChoiceQuestions where Id in @Ids";

            var Ids = questions.Select(x => x.Id);

            return await _context.Conn.ExecuteAsync(sql, new { Ids }, transaction: _context.Transaction);

        }

        public async Task<int> DeleteMany(IEnumerable<MultipleChoiceQuestionDto> questions)
        {
            var sql = @"delete from dbo.MultipleChoiceQuestions where Id in @Ids";

            var Ids = questions.Select(x => x.Id);

            return await _context.Conn.ExecuteAsync(sql, new { Ids }, transaction: _context.Transaction);
        }

        public async Task<int> DeleteMany(IEnumerable<TextAnswerQuestionDto> questions)
        {
            var sql = @"delete from dbo.TextAnswerQuestions where Id in @Ids";

            var Ids = questions.Select(x => x.Id);

            return await _context.Conn.ExecuteAsync(sql, new { Ids }, transaction: _context.Transaction);
        }

        public async Task<int> InsertMany(IEnumerable<SingleChoiceQuestion> questions)
        {
            return await _context.Conn.ExecuteAsync(
            "dbo.spSingleChoiceQuestions_Insert",
            questions,
            transaction: _context.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertMany(IEnumerable<MultipleChoiceQuestion> questions)
        {
            return await _context.Conn.ExecuteAsync(
            "dbo.spMultipleChoiceQuestions_Insert",
            questions,
            transaction: _context.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertMany(IEnumerable<TextAnswerQuestion> questions)
        {
            return await _context.Conn.ExecuteAsync(
            "dbo.spTextAnswerQuestions_Insert",
            questions,
            transaction: _context.Transaction,
            commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateMany(IEnumerable<SingleChoiceQuestionDto> questions)
        {
            // Prepare params
            var parameters = questions.Select(x => new
            {
                x.Id,
                x.QuestionText,
                x.QuestionType,
                x.Number,
                x.Choices
            });

            return await _context.Conn.ExecuteAsync("dbo.spSingleChoiceQuestions_Update",
                parameters,
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateMany(IEnumerable<MultipleChoiceQuestionDto> questions)
        {
            // Prepare params 
            var parameters = questions.Select(x => new
            {
                x.Id,
                x.QuestionText,
                x.QuestionType,
                x.Number,
                x.Choices
            });

            return await _context.Conn.ExecuteAsync("dbo.spMultipleChoiceQuestions_Update",
                parameters,
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateMany(IEnumerable<TextAnswerQuestionDto> questions)
        {
            // Prepare params
            var parameters = questions.Select(x => new
            {
                x.Id,
                x.QuestionText,
                x.QuestionType,
                x.Number,
            });

            return await _context.Conn.ExecuteAsync("dbo.spTextAnswerQuestions_Update",
                parameters,
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
