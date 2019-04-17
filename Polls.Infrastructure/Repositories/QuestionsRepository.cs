using Dapper;
using Polls.Core.Domain;
using Polls.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polls.Infrastructure.Repositories
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private IPollsRepository _pollsRepository;

        public QuestionsRepository(IPollsRepository pollsRepo)
        {
            _pollsRepository = pollsRepo;
        }
        public async Task<Dictionary<string, List<Answer>>> GetQuestionsWithAnswers(int pollId)
        {
            // Returs two result sets, list of TextAnswers and SingleChoiceAnswers.
            var answersSql = @"SELECT ta.Id, ta.QuestionId, ta.Answer
                                    FROM TextAnswers ta
                                    JOIN TextAnswerQuestions taQ on ta.QuestionId = taQ.Id
                                    WHERE taQ.PollId = @pollId;
                               SELECT scA.Id, scA.QuestionId, scA.Choice FROM SingleChoiceAnswers scA
                                    JOIN SingleChoiceQuestions scQ on scQ.Id = scA.QuestionId
                                    WHERE scQ.PollId = @pollId";


            var result = new Dictionary<string, List<Answer>>();

            using (var cnn = Connection.GetConnection())
            {
                var answersReader = await cnn.QueryMultipleAsync(answersSql, new { pollId });

                var answers = new List<Answer>();

                answers.AddRange(answersReader.Read<TextAnswer>());
                answers.AddRange(answersReader.Read<SingleChoiceAnswer>());

                cnn.Close();

                foreach (var answer in answers)
                {
                    if (result.ContainsKey(answer.QuestionId))
                    {
                        result[answer.QuestionId].Add(answer);
                    }
                    else
                    {
                        result.Add(answer.QuestionId, new List<Answer>() { answer });
                    }
                }
            }

            return result;
        }
    }
}

