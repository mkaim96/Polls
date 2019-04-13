using MediatR;
using Polls.Core.Repositories;
using Polls.Core.Statistics;
using Polls.Infrastructure.Commands.Polls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class GenerateStatisticsHandler : IRequestHandler<GenerateStatistics, IEnumerable<QuestionStatistics>>
    {
        private IPollsRepository _pollsRepository;
        private IQuestionsRepository _questionsRepository;

        public GenerateStatisticsHandler(IPollsRepository pollsRepo, IQuestionsRepository questionsRepo)
        {
            _pollsRepository = pollsRepo;
            _questionsRepository = questionsRepo;
        }
        public async Task<IEnumerable<QuestionStatistics>> Handle(GenerateStatistics request, CancellationToken cancellationToken)
        {
            var poll = await _pollsRepository.Get(request.PollId);
            var answers = await _questionsRepository.GetQuestionsWithAnswers(request.PollId);

            var stats = new List<QuestionStatistics>();

            foreach(var question in poll.Questions)
            {
                if(answers.ContainsKey(question.Id))
                {
                    var questionStats = question.GenerateStatistics(answers[question.Id]);
                    stats.Add(questionStats);
                }
            }

            return stats;
        }
    }
}
