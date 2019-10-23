using Dapper;
using MediatR;
using Microsoft.Extensions.Primitives;
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

            // Initialize lists of answers.
            var scQuestionAnswers = new List<SingleChoiceAnswer>();
            var taQuestionAnswers = new List<TextAnswer>();
            var mcQuestionAnswers = new List<MultipleChoiceAnswer>();

            // Loop through questions and depending on the type create answer for it 
            foreach (var question in poll.Questions)
            {
                switch (question)
                {
                    case SingleChoiceQuestion q:

                        // Check if passed answers contains answer for this question
                        if (request.Form.ContainsKey(q.Id.ToString()))
                        {
                            scQuestionAnswers.Add(new SingleChoiceAnswer
                            {
                                QuestionId = q.Id,
                                Choice = request.Form[question.Id.ToString()],
                            });
                        }
                        break;

                    case MultipleChoiceQuestion q:
                        if (request.Form.ContainsKey(q.Id.ToString()))
                        {
                            mcQuestionAnswers.Add(new MultipleChoiceAnswer
                            {
                                QuestionId = q.Id,
                                Choices = request.Form[q.Id.ToString()]
                            });
                        }
                        break;

                    case TextAnswerQuestion q:
                        if (request.Form.ContainsKey(q.Id.ToString()))
                        {
                            // Check if answer isnt empty
                            if (StringValues.IsNullOrEmpty(request.Form[q.Id.ToString()]))
                            {
                                continue;
                            }

                            taQuestionAnswers.Add(new TextAnswer
                            {
                                QuestionId = q.Id,
                                Answer = request.Form[q.Id.ToString()]
                            });
                        }
                        break;
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


