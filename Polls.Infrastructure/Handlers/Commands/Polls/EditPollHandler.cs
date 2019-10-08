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
using Polls.Infrastructure.Repositories;
using Polls.Core.Domain;
using Polls.Infrastructure.UnitOfWork;
using AutoMapper;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class EditPollHandler : AsyncRequestHandler<EditPoll>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditPollHandler(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }
        protected override async Task Handle(EditPoll request, CancellationToken cancellationToken)
        {

            var tasks = new List<Task>();

            #region Deleting questions
            // Delete single choice questions.
            if (request.ScQuestionsToDelete.Count > 0)
            {
                var questions = _mapper.Map<IEnumerable<SingleChoiceQuestion>>(request.ScQuestionsToDelete);

                var t1 = _unitOfWork.Questions.DeleteMany(questions);
                tasks.Add(t1);
            }

            // Delete text asnwer questions.
            if (request.TaQuestionsToDelete.Count > 0)
            {
                var questions = _mapper.Map<IEnumerable<TextAnswerQuestion>>(request.TaQuestionsToDelete);

                var t2 = _unitOfWork.Questions.DeleteMany(questions);
                tasks.Add(t2);
            }

            // Delete multiple choice questions.
            if(request.McQuestionsToDelete.Count > 0)
            {
                var questions = _mapper.Map<IEnumerable<MultipleChoiceQuestion>>(request.McQuestionsToDelete);

                var t3 = _unitOfWork.Questions.DeleteMany(questions);
            }
            #endregion

            #region Inserting new questions
            // Insert single choice questions.
            if (request.NewScQuestions.Count > 0)
            {
                // Prepare questions.
                var questions = request.NewScQuestions.Select(x => new SingleChoiceQuestion(
                    request.PollId, x.QuestionType, x.QuestionText, x.Number, x.Choices
                    )
                );

                var t4 = _unitOfWork.Questions.InsertMany(questions);
                tasks.Add(t4);
            }

            // Insert text answer questions.
            if (request.NewTaQuestions.Count > 0)
            {
                // Prepare questions.
                var questions = request.NewTaQuestions.Select(x => new TextAnswerQuestion(
                        request.PollId, x.QuestionType, x.QuestionText, x.Number
                        )
                );

                var t5 = _unitOfWork.Questions.InsertMany(questions);
                tasks.Add(t5);
            }

            // Insert multiple choice questions
            if(request.NewMcQuestions.Count > 0)
            {
                // Prepare questions.
                var questions = request.NewMcQuestions.Select(x => new MultipleChoiceQuestion(
                        request.PollId, x.QuestionType, x.QuestionText, x.Number, x.Choices
                        )
                );

                var t6 = _unitOfWork.Questions.InsertMany(questions);
                tasks.Add(t6);
            }
            #endregion

            #region Updating questions
            // Update Single Choice Questions.
            if(request.ScQuestionsToUpdate.Count > 0)
            {
                var questions = _mapper.Map<IEnumerable<SingleChoiceQuestion>>(request.ScQuestionsToUpdate);

                var t7 = _unitOfWork.Questions.UpdateMany(questions);
                tasks.Add(t7);
            }

            // Update Text Answer Questions.
            if(request.TaQuestionsToUpdate.Count > 0)
            {
                var questions = _mapper.Map<IEnumerable<TextAnswerQuestion>>(request.TaQuestionsToUpdate);

                var t8 = _unitOfWork.Questions.UpdateMany(questions);
                tasks.Add(t8);

            }

            // Update multiple choice questions.
            if(request.McQuestionsToUpdate.Count > 0)
            {
                var questions = _mapper.Map<IEnumerable<MultipleChoiceQuestion>>(request.McQuestionsToUpdate);

                var t9 = _unitOfWork.Questions.UpdateMany(questions);
                tasks.Add(t9);
            }
            #endregion

            // Update poll.
            var t10 = _unitOfWork.Polls.Update(request.PollId, request.NewTitle, request.NewDescription);
            tasks.Add(t10);


            await Task.WhenAll(tasks);
            _unitOfWork.Complete();
        }
    }
}
