using Dapper;
using MediatR;
using Polls.Infrastructure.Commands.Polls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Handlers.Commands.Polls
{
    public class ClearAnswersHandler : AsyncRequestHandler<ClearAnswers>
    {
        protected override async Task Handle(ClearAnswers request, CancellationToken cancellationToken)
        {
            var deleteScaSql = @"DELETE sca 
                        FROM dbo.SingleChoiceAnswers sca 
                        JOIN dbo.SingleChoiceQuestions scq on sca.QuestionId = scq.Id
                        JOIN dbo.Polls p on scq.PollId = p.Id
                        WHERE p.Id = @PollId";

            var deleteMcaSql = @"DELETE mca
                        FROM dbo.MultipleChoiceAnswers mca
                        JOIN dbo.MultipleChoiceQuestions mcq on mca.QuestionId = mcq.Id
                        JOIN dbo.Polls p on mcq.PollId = p.Id";

            var deleteTaSql = @"DELETE ta
                        FROM dbo.TextAnswers ta
                        JOIN dbo.TextAnswerQuestions taq on ta.QuestionId = taq.Id
                        JOIN dbo.Polls p on taq.PollId = p.Id";


            using(var cnn = Connection.GetConnection())
            {
                cnn.Open();
                var tr = cnn.BeginTransaction();

                var t1 = cnn.ExecuteAsync(deleteScaSql, new { request.PollId }, transaction: tr);
                var t2 = cnn.ExecuteAsync(deleteMcaSql, new { request.PollId }, transaction: tr);
                var t3 = cnn.ExecuteAsync(deleteTaSql, new { request.PollId }, transaction: tr);

                await Task.WhenAll(t1, t2, t3);

                try
                {
                    tr.Commit();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Commit Exception type: {0}", ex.GetType());
                    Console.WriteLine(" Message: {0}", ex.Message);

                    tr.Rollback();
                }

            }
        }
    }
}
