using MediatR;
using Polls.Core.Repositories;
using Polls.Core.Domain;
using Polls.Infrastructure.Commands.Polls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Polls.Infrastructure.Dto;
using System.Linq;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class CreatePollHandler : AsyncRequestHandler<CreatePoll>
    {
        protected override async Task Handle(CreatePoll request, CancellationToken cancellationToken)
        {
            #region SQL

            // sql for inserting poll and return id of created poll
            var pollSql = @"INSERT INTO dbo.Polls (Title, Description, UserId) 
                    OUTPUT INSERTED.Id
                    VALUES (@Title, @Description, @UserId)";

            // sql for inserting SingleChoiceQuestion 
            var scQuestionSql = @"INSERT INTO 
                    dbo.SingleChoiceQuestions (Id, QuestionText, QuestionType, Choices, PollId, Number)
                    VALUES (@Id, @QuestionText, @QuestionType, @Choices, @PollId, @Number)";

            // sql for inserting TextAnswerQuestion
            var taQuestionSql = @"INSERT INTO 
                    dbo.TextAnswerQuestions (Id, QuestionText, QuestionType, PollId, Number)
                    VALUES (@Id, @QuestionText, @QuestionType, @PollId, @Number)";

            #endregion  

            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();
 
                using (var tr = cnn.BeginTransaction())
                {
                    // Insert poll and get Id of inserted row
                    var pollId = await cnn.QuerySingleAsync<int>(pollSql, 
                        new { request.Title, request.Description, request.UserId },
                        transaction: tr);

                    // insert SingleChoiceQuestions if any
                    if (request.SingleChoiceQuestions != null)
                    {
                        await cnn.ExecuteAsync(scQuestionSql,
                            MapSingleChoiceQuestions(request.SingleChoiceQuestions, pollId),
                            transaction: tr);
                    }

                    // insert TextAnswerQuestions if any
                    if (request.TextAnswerQuestions != null)
                    {
                        await cnn.ExecuteAsync(taQuestionSql,
                            MapTextAnswerQuestions(request.TextAnswerQuestions, pollId),
                            transaction: tr);
                    }

                    tr.Commit();
                }
            }
        }

        // Maps SingleChoiceQuestionsDto to match parameters in sql query.
        private IEnumerable<object> MapSingleChoiceQuestions(IEnumerable<SingleChoiceQuestionDto> questions, int pollId)
        {
            return questions.Select(x => new
            {
                Id = Guid.NewGuid().ToString(),
                QuestionText = x.QuestionText,
                QuestionType = x.QuestionType.ToString(),
                Choices = string.Join(",", x.Choices),
                PollId = pollId,
                Number = x.Number
            });
        }

        // Maps TextAnswerQuestionsDto to match parameters in sql query.
        private IEnumerable<object> MapTextAnswerQuestions(IEnumerable<TextAnswerQuestionDto> questions, int pollId)
        {
            return questions.Select(x => new
            {
                Id = Guid.NewGuid().ToString(),
                QuestionText = x.QuestionText,
                QuestionType = x.QuestionType.ToString(),
                PollId = pollId,
                Number = x.Number
            });
        }
    }
}
