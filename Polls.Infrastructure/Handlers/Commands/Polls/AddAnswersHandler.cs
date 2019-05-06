using Dapper;
using MediatR;
using Polls.Core.Domain;
using Polls.Core.Repositories;
using Polls.Infrastructure;
using Polls.Infrastructure.Commands.Polls;
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
        private IPollsRepository _pollsRepository;

        public AddAnswersHandler(IPollsRepository pollsRepo)
        {
            _pollsRepository = pollsRepo;
        }
        protected override async Task Handle(AddAnswers request, CancellationToken cancellationToken)
        {
            var pollId = Convert.ToInt32(request.Form["PollId"]);

            var poll = await _pollsRepository.Get(pollId);

            var scQuestions = poll.GetConcreteQuestions<SingleChoiceQuestion>();
            var taQuestions = poll.GetConcreteQuestions<TextAnswerQuestion>();

            // Initialize lists of answers.
            var scQuestionAnswers = new List<SingleChoiceAnswer>();
            var taQuestionAnswers = new List<TextAnswer>();

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


            // Insert answers to database.
            var scAnswerSql = @"INSERT INTO dbo.SingleChoiceAnswers (QuestionId, Choice) VALUES (@QuestionId, @Choice)";
            var textAnswerSql = @"INSERT INTO dbo.TextAnswers (QuestionId, Answer) VALUES (@QuestionId, @Answer)";

            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                using (var tr = cnn.BeginTransaction())
                {
                    cnn.Execute(scAnswerSql, scQuestionAnswers, transaction: tr);
                    cnn.Execute(textAnswerSql, taQuestionAnswers, transaction: tr);

                    tr.Commit();
                }

                cnn.Close();
            }


        }
    }
}


