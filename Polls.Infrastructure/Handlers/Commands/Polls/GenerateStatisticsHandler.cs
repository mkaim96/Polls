using MediatR;
using Polls.Core.Domain;
using Polls.Core.Statistics;
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
    public class GenerateStatisticsHandler : IRequestHandler<GenerateStatistics, IEnumerable<QuestionStatistics>>
    {
        public async Task<IEnumerable<QuestionStatistics>> Handle(GenerateStatistics request, CancellationToken cancellationToken)
        {
            Poll poll;
            IEnumerable<Answer> answers = new List<Answer>();
            var tasks = new List<Task>();

            using (var cnn = Connection.GetConnection())
            {
                cnn.Open();

                var tr = cnn.BeginTransaction();
                var repo = new QueriesRepoUoW(tr);

                var t1 = repo.Get(request.PollId);
                tasks.Add(t1);

                var t2 = repo.GetAnswers(request.PollId);
                tasks.Add(t2);

                await Task.WhenAll(tasks);
                tr.Commit();

                poll = t1.Result;
                answers = t2.Result;
            }

            // Answers grouped by id of questions.
            var dict = new Dictionary<string, List<Answer>>();

            // Group answers.
            foreach (var answer in answers)
            {
                if (dict.ContainsKey(answer.QuestionId))
                {
                    dict[answer.QuestionId].Add(answer);
                }
                else
                {
                    dict.Add(answer.QuestionId, new List<Answer> { answer });
                }
            }

            var stats = new List<QuestionStatistics>();

            // Loop through each question and generate statistics for it.
            foreach (var question in poll.Questions)
            {
                if (dict.ContainsKey(question.Id))
                {
                    var questionStats = question.GenerateStatistics(dict[question.Id]);
                    stats.Add(questionStats);
                }
            }

            
            return stats.OrderBy(x => x.Question.Number);
        }
    }
}
