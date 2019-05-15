using Dapper;
using MediatR;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class EditPollHandler : AsyncRequestHandler<EditPoll>
    {
        protected override async Task Handle(EditPoll request, CancellationToken cancellationToken)
        {
            #region SQL

            var removeScQuestions = @"delete from dbo.SingleChoiceQuestions where Id in @Ids";
            var removeTaQuestions = @"delete from dbo.TextAnswerQuestions where Id in @Ids";

            #endregion

            using (var cnn = Connection.GetConnection())
            {
                var tasks = new List<Task>();
                cnn.Open();

                using (var tr = cnn.BeginTransaction())
                {
                    // Delete single choice questions if any to delete.
                    if (request.ScQuestionsToDelete != null)
                    {
                        var Ids = request.ScQuestionsToDelete.Select(x => x.Id).ToList();
                        tasks.Add(cnn.ExecuteAsync(removeScQuestions, new { Ids }, transaction: tr));
                    }

                    // Delete text asnwer questions if any.
                    if (request.TaQuestionsToDelete != null)
                    {
                        var Ids = request.TaQuestionsToDelete.Select(x => x.Id).ToList();
                        tasks.Add(cnn.ExecuteAsync(removeTaQuestions, new { Ids }, transaction: tr));
                    }

                    // Insert single choice questions.
                    if (request.NewScQuestions != null)
                    {
                        // Parametrs for dapper.
                        var parameters = request.NewScQuestions.Select(x => new
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionText = x.QuestionText,
                            QuestionType = x.QuestionType.ToString(),
                            Choices = string.Join(",", x.Choices),
                            PollId = request.PollId,
                            Number = x.Number
                        });

                        var task = cnn.ExecuteAsync("dbo.spSingleChoiceQuestions_Insert",
                            parameters,
                            transaction: tr,
                            commandType: CommandType.StoredProcedure);

                        tasks.Add(task);
                    }

                    // Insert texta answer questions.
                    if(request.NewTaQuestions != null)
                    {
                        // Parameters for dapper
                        var parameters = request.NewTaQuestions.Select(x => new
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionText = x.QuestionText,
                            QuestionType = x.QuestionType.ToString(),
                            PollId = request.PollId,
                            Number = x.Number
                        });

                        var task = cnn.ExecuteAsync("dbo.spTextAnswerQuestions_Insert",
                            parameters,
                            transaction: tr,
                            commandType: CommandType.StoredProcedure);

                        tasks.Add(task);
                    }

                    // Upadte poll
                    var updatePollParams = new { request.NewTitle, request.NewDescription, request.PollId };

                    var updatePollTask = cnn.ExecuteAsync("dbo.spPolls_Update",
                        updatePollParams,
                        transaction: tr,
                        commandType: CommandType.StoredProcedure);

                    tasks.Add(updatePollTask);

                    //Update Single Choice Questions
                    var updateScQParams = request.ScQuestionsToUpdate.Select(x => new
                    {
                        x.QuestionText,
                        x.QuestionType,
                        Choices = string.Join(",", x.Choices),
                        x.Number,
                        x.Id
                    });

                    var updateScQTask = cnn.ExecuteAsync("dbo.spSingleChoiceQuestions_Update",
                        updateScQParams,
                        transaction: tr,
                        commandType: CommandType.StoredProcedure);

                    tasks.Add(updateScQTask);

                    // Update Text Answer Questions

                    // Prepare params
                    var updateTaQParams = request.TaQuestionsToUpdate.Select(x => new
                    {
                        x.QuestionText,
                        x.QuestionType,
                        x.Number,
                        x.Id
                    });

                    var updateTaQTask = cnn.ExecuteAsync("dbo.spTextAnswerQuestions_Update",
                        updateTaQParams,
                        transaction: tr,
                        commandType: CommandType.StoredProcedure);

                    tasks.Add(updateTaQTask);

                    await Task.WhenAll(tasks);

                    tr.Commit();
                }
            }
        }
    }
}
