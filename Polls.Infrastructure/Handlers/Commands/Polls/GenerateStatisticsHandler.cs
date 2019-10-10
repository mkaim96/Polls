using MediatR;
using Polls.Core.Domain;
using Polls.Core.Statistics;
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
    public class GenerateStatisticsHandler : IRequestHandler<GenerateStatistics, IEnumerable<QuestionStatistics>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenerateStatisticsHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<IEnumerable<QuestionStatistics>> Handle(GenerateStatistics request, CancellationToken cancellationToken)
        {
            // Run tasks asynchronously.
            var t1 = _unitOfWork.Polls.Get(request.PollId);
            var t2 = _unitOfWork.Answers.GetAll(request.PollId);


            await Task.WhenAll(t1, t2);
            _unitOfWork.Complete();

            // When tasks are finished assign result to variables.
            var poll = t1.Result;
            var answers = t2.Result;

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
                else
                {
                    var s = question.GenerateStatistics(new List<Answer>());
                    stats.Add(s);
                }
            }
            return stats.OrderBy(x => x.Question.Number);
        }
    }
}
