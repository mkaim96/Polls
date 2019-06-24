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
using Polls.Core.Repositories;
using Polls.Infrastructure.Repositories;
using Polls.Core.Domain;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class EditPollHandler : AsyncRequestHandler<EditPoll>
    {
        protected override async Task Handle(EditPoll request, CancellationToken cancellationToken)
        {
            #region SQL

            var removeScQuestions = @"delete from dbo.SingleChoiceQuestions where Id in @Ids";
            var removeTaQuestions = @"delete from dbo.TextAnswerQuestions where Id in @Ids";
            var removeMcQuestions = @"delete from dbo.MultipleChoiceQuestions where Id in @Ids";

            #endregion

            using (var cnn = Connection.GetConnection())
            {
                var tasks = new List<Task>();
                cnn.Open();

                var tr = cnn.BeginTransaction();

                IPollsRepository pollsRepo = new PollsRepositoryTr(tr);
                IQuestionsRepository questionsRepo = new QuestionsRepositoryTr(tr);

                try
                {
                    // Delete single choice questions.
                    if (request.ScQuestionsToDelete.Count > 0)
                    {
                        var Ids = request.ScQuestionsToDelete.Select(x => x.Id).ToList();
                        tasks.Add(cnn.ExecuteAsync(removeScQuestions, new { Ids }, transaction: tr));
                    }

                    // Delete text asnwer questions.
                    if (request.TaQuestionsToDelete.Count > 0)
                    {
                        var Ids = request.TaQuestionsToDelete.Select(x => x.Id).ToList();
                        tasks.Add(cnn.ExecuteAsync(removeTaQuestions, new { Ids }, transaction: tr));
                    }

                    // Delete multiple choice questions.
                    if(request.McQuestionsToDelete.Count > 0)
                    {
                        var Ids = request.TaQuestionsToDelete.Select(x => x.Id).ToList();
                        tasks.Add(cnn.ExecuteAsync(removeMcQuestions, new { Ids }, transaction: tr));
                    }

                    // Insert single choice questions.
                    if (request.NewScQuestions.Count > 0)
                    {
                        // Prepare questions.
                        var questions = request.NewScQuestions.Select(x =>
                        {
                            return new SingleChoiceQuestion(
                                request.PollId, x.QuestionType, x.QuestionText, x.Number, x.Choices
                                );
                        });

                        var task = questionsRepo.Insert(questions);

                        tasks.Add(task);
                    }

                    // Insert text answer questions.
                    if (request.NewTaQuestions.Count > 0)
                    {
                        // Prepare questions.
                        var questions = request.NewTaQuestions.Select(x =>
                        {
                            return new TextAnswerQuestion(
                                request.PollId, x.QuestionType, x.QuestionText, x.Number
                                );
                        });

                        var task = questionsRepo.Insert(questions);

                        tasks.Add(task);
                    }

                    // Insert multiple choice questions
                    if(request.NewMcQuestions.Count > 0)
                    {
                        // Prepare questions.
                        var questions = request.NewMcQuestions.Select(x =>
                        {
                            return new MultipleChoiceQuestion(request.PollId, x.QuestionType, x.QuestionText, x.Number, x.Choices);
                        });

                        var task = questionsRepo.Insert(questions);

                        tasks.Add(task);
                    }

                    // Update Single Choice Questions.
                    if(request.ScQuestionsToUpdate.Count > 0)
                    {
                        // Prepare params
                        var singleChoiceQuestionsToUpdate = request.ScQuestionsToUpdate.Select(x => new
                        {
                            x.Id,
                            x.QuestionText,
                            x.QuestionType,
                            x.Number,
                            x.Choices
                        });

                        var updateScQTask = cnn.ExecuteAsync("dbo.spSingleChoiceQuestions_Update",
                            singleChoiceQuestionsToUpdate,
                            transaction: tr,
                            commandType: CommandType.StoredProcedure);

                        tasks.Add(updateScQTask);
                    }

                    // Update Text Answer Questions.
                    if(request.TaQuestionsToUpdate.Count > 0)
                    {
                        // Prepare params
                        var textAnswerQuestionsToUpdate = request.TaQuestionsToUpdate.Select(x => new
                        {
                            x.Id,
                            x.QuestionText,
                            x.QuestionType,
                            x.Number,
                        });

                        var updateTaQTask = cnn.ExecuteAsync("dbo.spTextAnswerQuestions_Update",
                            textAnswerQuestionsToUpdate,
                            transaction: tr,
                            commandType: CommandType.StoredProcedure);

                        tasks.Add(updateTaQTask);

                    }

                    // Update multiple choice questions.
                    if(request.McQuestionsToUpdate.Count > 0)
                    {
                        // Prepare params 
                        var multipleChoiceQuestionsToUpdate = request.McQuestionsToUpdate.Select(x => new
                        {
                            x.Id,
                            x.QuestionText,
                            x.QuestionType,
                            x.Number,
                            x.Choices
                        });

                        var task = cnn.ExecuteAsync("dbo.spMultipleChoiceQuestions_Update", multipleChoiceQuestionsToUpdate, transaction: tr);

                        tasks.Add(task);
                    }


                    // Upadte poll.
                    var poll = await cnn.QuerySingleAsync<Poll>(
                        "select * from dbo.Polls where Id = @PollId",
                        new { request.PollId },
                        transaction: tr);

                    poll.SetTitle(request.NewTitle);
                    poll.SetDescription(request.NewDescription);

                    var updatePollTask = pollsRepo.Update(poll);

                    tasks.Add(updatePollTask);

                    await Task.WhenAll(tasks);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception type: {0}", ex.GetType());
                    Console.WriteLine(" Message: {0}", ex.Message);

                    tr.Rollback();
                }
            }
        }
    }
}
