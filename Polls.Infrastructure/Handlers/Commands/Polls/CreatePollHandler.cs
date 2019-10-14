using MediatR;
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
using Polls.Infrastructure.UnitOfWork;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class CreatePollHandler : AsyncRequestHandler<CreatePoll>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePollHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        protected override async Task Handle(CreatePoll request, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            // Create poll
            var poll = new Poll(request.UserId, request.Title, request.Description);
            var pollId = await _unitOfWork.Polls.Add(poll);

            // Insert SingleChoiceQuestions if any
            if (request.SingleChoiceQuestions.Count > 0)
            {
                // Prepare questions
                var questions = request.SingleChoiceQuestions.Select(x =>
                {
                    return new SingleChoiceQuestion(
                        pollId, x.QuestionText, x.Number, x.Choices
                        );
                }).ToList();

                var t1 = _unitOfWork.Questions.InsertMany(questions);
                tasks.Add(t1);
            }

            // Insert MultipleCHoiceQuestions if any
            if (request.MultipleChoiceQuestions.Count > 0)
            {
                // Prepare questions
                var questions = request.MultipleChoiceQuestions.Select(x =>
                {
                    return new MultipleChoiceQuestion(
                        pollId, x.QuestionText, x.Number, x.Choices
                        );
                }).ToList();

                var t2 = _unitOfWork.Questions.InsertMany(questions);
                tasks.Add(t2);
            }

            // Insert TextAnswerQuestions if any
            if (request.TextAnswerQuestions.Count > 0)
            {
                // Prepare questions
                var questions = request.TextAnswerQuestions.Select(x =>
                {
                    return new TextAnswerQuestion(
                        pollId, x.QuestionText, x.Number
                        );
                }).ToList();

                var t3 = _unitOfWork.Questions.InsertMany(questions);
                tasks.Add(t3);

            }

            await Task.WhenAll(tasks);

            _unitOfWork.Complete();





            //using (var cnn = Connection.GetConnection())
            //{
            //    var tasks = new List<Task>();

            //    cnn.Open();

            //    var tr = cnn.BeginTransaction();

            //    try
            //    {
            //        var pollParams = new { request.UserId, request.Title, request.Description };
            //        // Insert poll and get Id of inserted row
            //        var pollId = await cnn.QuerySingleAsync<int>(
            //            "dbo.spPolls_Insert",
            //            pollParams,
            //            transaction: tr,
            //            commandType: CommandType.StoredProcedure);

            //        // insert SingleChoiceQuestions if any
            //        if (request.SingleChoiceQuestions.Count > 0)
            //        {
            //            // Prepare questions
            //            var questions = request.SingleChoiceQuestions.Select(x =>
            //            {
            //                return new SingleChoiceQuestion(
            //                    pollId, x.QuestionType, x.QuestionText, x.Number, x.Choices
            //                    );
            //            }).ToList();

            //            var task = cnn.ExecuteAsync(
            //                "dbo.spSingleChoiceQuestions_Insert",
            //                questions,
            //                transaction: tr,
            //                commandType: CommandType.StoredProcedure);

            //            tasks.Add(task);
            //        }

            //        // insert MultipleCHoiceQuestions if any
            //        if(request.MultipleChoiceQuestions.Count > 0)
            //        {
            //            var questions = request.MultipleChoiceQuestions.Select(x =>
            //            {
            //                return new MultipleChoiceQuestion(
            //                    pollId, x.QuestionType, x.QuestionText, x.Number, x.Choices
            //                    );
            //            }).ToList();

            //            var task = cnn.ExecuteAsync(
            //                "dbo.spMultipleChoiceQuestions_Insert",
            //                questions,
            //                transaction: tr,
            //                commandType: CommandType.StoredProcedure);

            //            tasks.Add(task);
            //        }

            //        // insert TextAnswerQuestions if any
            //        if (request.TextAnswerQuestions.Count > 0)
            //        {
            //            var questions = request.TextAnswerQuestions.Select(x =>
            //            {
            //                return new TextAnswerQuestion(
            //                    pollId, x.QuestionType, x.QuestionText, x.Number
            //                    );
            //            }).ToList();

            //            var task = cnn.ExecuteAsync(
            //                "dbo.spTextAnswerQuestions_Insert",
            //                questions,
            //                transaction: tr,
            //                commandType: CommandType.StoredProcedure);

            //            tasks.Add(task);

            //        }

            //        await Task.WhenAll(tasks);

            //        tr.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Commit Exception type: {0}", ex.GetType());
            //        Console.WriteLine(" Message: {0}", ex.Message);

            //        tr.Rollback();
            //    }
            //}
        }
    }
}
