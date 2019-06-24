using Dapper;
using Polls.Core.Domain;
using Polls.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Repositories
{
    public class QuestionsRepositoryTr : IQuestionsRepository
    {
        IDbTransaction tr;
        IDbConnection cnn => tr.Connection;

        public QuestionsRepositoryTr(IDbTransaction transaction)
        {
            tr = transaction;
        }

        public async Task<int> Insert(IEnumerable<SingleChoiceQuestion> questions)
        {
            return await cnn.ExecuteAsync("dbo.spSingleChoiceQuestions_Insert",
                questions,
                transaction: tr,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Insert(IEnumerable<TextAnswerQuestion> questions)
        {
            return await cnn.ExecuteAsync("dbo.spTextAnswerQuestions_Insert",
                questions,
                transaction: tr,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Update(IEnumerable<SingleChoiceQuestion> questions)
        {
            return await cnn.ExecuteAsync("dbo.spSingleChoiceQuestions_Update",
                questions,
                transaction: tr,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Update(IEnumerable<TextAnswerQuestion> questions)
        {
            return await cnn.ExecuteAsync("dbo.spTextAnswerQuestions_Update",
                questions,
                transaction: tr,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Insert(IEnumerable<MultipleChoiceQuestion> questions)
        {
            return await cnn.ExecuteAsync("dbo.spMultipleChoiceQuestions_Insert",
                questions,
                transaction: tr,
                commandType: CommandType.StoredProcedure
                );
        }

        public Task<int> Update(IEnumerable<MultipleChoiceQuestion> questions)
        {
            throw new NotImplementedException();
        }
    }
}
