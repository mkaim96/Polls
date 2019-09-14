using Dapper;
using MediatR;
using Polls.Core.Domain;
using Polls.Infrastructure;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Repositories;
using Polls.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class AddAnswersHandler : AsyncRequestHandler<AddAnswers>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAnswersHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        protected override async Task Handle(AddAnswers request, CancellationToken cancellationToken)
        {
            var pollId = Convert.ToInt32(request.Form["PollId"]);

            var poll = await _unitOfWork.Polls.Get(pollId);

            var scQuestions = poll.GetConcreteQuestions<SingleChoiceQuestion>();
            var taQuestions = poll.GetConcreteQuestions<TextAnswerQuestion>();
            var mcQuestions = poll.GetConcreteQuestions<MultipleChoiceQuestion>();


            // Initialize lists of answers.
            var scQuestionAnswers = new List<SingleChoiceAnswer>();
            var taQuestionAnswers = new List<TextAnswer>();
            var mcQuestionAnswers = new List<MultipleChoiceAnswer>();

            // Loop through every single choice question and and check if form contains answer for it
            // if yes, create answer and add to list.
            foreach (var question in scQuestions)
            {
                if (request.Form.ContainsKey(question.Id.ToString()))
                {
                    scQuestionAnswers.Add(new SingleChoiceAnswer
                    {
                        QuestionId = question.Id,
                        Choice = request.Form[question.Id.ToString()],
                    });
                }
            }
            
            foreach (var question in mcQuestions)
            {
                if(request.Form.ContainsKey(question.Id.ToString()))
                {
                    mcQuestionAnswers.Add(new MultipleChoiceAnswer
                    {
                        QuestionId = question.Id,
                        Choices = request.Form[question.Id.ToString()]
                    });
                }
            }

            // Loop through every text answer question and and check if form contains answer for it
            // if yes, create answer and add to list.
            foreach (var question in taQuestions)
            {
                if (request.Form.ContainsKey(question.Id.ToString()))
                {
                    // Check if answer isnt empty
                    if (String.IsNullOrEmpty(request.Form[question.Id.ToString()]))
                    {
                        continue;
                    }

                    taQuestionAnswers.Add(new TextAnswer
                    {
                        QuestionId = question.Id,
                        Answer = request.Form[question.Id.ToString()]
                    });
                }
            }

            // Insert prepared lists of answers to database
            var t1 = _unitOfWork.Answers.InsertMany(scQuestionAnswers);
            var t2 = _unitOfWork.Answers.InsertMany(taQuestionAnswers);
            var t3 = _unitOfWork.Answers.InsertMany(mcQuestionAnswers);

            await Task.WhenAll(t1, t2, t3);

            _unitOfWork.Complete();
        }
    }
}


