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
using System.Data;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class CreatePollHandler : AsyncRequestHandler<CreatePoll>
    {
        protected override async Task Handle(CreatePoll request, CancellationToken cancellationToken)
        {
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                var tasks = new List<Task>();
                using (var tr = cnn.BeginTransaction())
                {
                    // Insert poll and get Id of inserted row
                    var pollId = await cnn.QuerySingleAsync<int>("dbo.spPolls_Insert", 
                        new { request.Title, request.Description, request.UserId },
                        transaction: tr,
                        commandType: CommandType.StoredProcedure);

                    // insert SingleChoiceQuestions if any
                    if (request.SingleChoiceQuestions != null)
                    {
                        // Prepare parameters for dapper
                        var parameters = request.SingleChoiceQuestions.Select(x => new
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionText = x.QuestionText,
                            QuestionType = x.QuestionType.ToString(),
                            Choices = string.Join(",", x.Choices),
                            PollId = pollId,
                            Number = x.Number
                        });

                        var t1 = cnn.ExecuteAsync("dbo.spSingleChoiceQuestions_Insert",
                            parameters,
                            transaction: tr,
                            commandType: CommandType.StoredProcedure);

                        tasks.Add(t1);
                    }

                    // insert TextAnswerQuestions if any
                    if (request.TextAnswerQuestions != null)
                    {
                        var parameters = request.TextAnswerQuestions.Select(x => new
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionText = x.QuestionText,
                            QuestionType = x.QuestionType.ToString(),
                            PollId = pollId,
                            Number = x.Number
                        });

                        var t2 = cnn.ExecuteAsync("dbo.spTextAnswerQuestions_Insert",
                            parameters,
                            transaction: tr,
                            commandType: CommandType.StoredProcedure);

                        tasks.Add(t2);

                    }
                    
                    await Task.WhenAll(tasks);

                    tr.Commit();

                }
            }
        }
    }
}
