﻿using Dapper;
using MediatR;
using Polls.Core.Domain;
using Polls.Core.Repositories;
using Polls.Infrastructure;
using Polls.Infrastructure.Commands.Polls;
using Polls.Infrastructure.Repositories;
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
        protected override async Task Handle(AddAnswers request, CancellationToken cancellationToken)
        {
            var pollId = Convert.ToInt32(request.Form["PollId"]);

            Poll poll;
            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();
                var tr = cnn.BeginTransaction();

                var repo = new QueriesRepoUoW(tr);

                poll = await repo.Get(pollId);
                tr.Commit();
                cnn.Close();
            }

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


            // Insert answers to database.
            var scAnswerSql = @"INSERT INTO dbo.SingleChoiceAnswers (QuestionId, Choice) VALUES (@QuestionId, @Choice)";
            var textAnswerSql = @"INSERT INTO dbo.TextAnswers (QuestionId, Answer) VALUES (@QuestionId, @Answer)";
            var mcAnswerSql = @"INSERT INTO dbo.MultipleChoiceAnswers (QuestionId, Choices) VALUES (@QuestionId, @Choices)";

            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                using (var tr = cnn.BeginTransaction())
                {
                    var t1 = cnn.ExecuteAsync(scAnswerSql, scQuestionAnswers, transaction: tr);
                    var t2 = cnn.ExecuteAsync(textAnswerSql, taQuestionAnswers, transaction: tr);

                    var t3 = cnn.ExecuteAsync(mcAnswerSql, mcQuestionAnswers, transaction: tr);

                    await Task.WhenAll(t1, t2, t3);

                    tr.Commit();
                }
            }


        }
    }
}


