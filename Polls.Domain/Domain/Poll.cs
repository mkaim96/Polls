using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polls.Core.Domain
{
    public class Poll
    {

        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public List<Question> Questions { get; private set; } = new List<Question>();

        protected Poll()
        { }

        public Poll(string userId, string title, string description)
        {
            UserId = userId;
            Title = title;
            Description = description;
        }

        public void AddQuestions(IEnumerable<Question> questions)
        {
            Questions.AddRange(questions);
        }

        public IEnumerable<T> GetConcreteQuestions<T>() where T : Question
        {
            var result = new List<T>();

            foreach(var question in Questions)
            {
                if(question is T q)
                {
                    result.Add(q);
                }
            }

            return result;
        }
    }
}
