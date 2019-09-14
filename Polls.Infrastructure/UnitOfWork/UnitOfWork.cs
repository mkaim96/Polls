using Polls.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polls.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PollsContext _context;

        public IPollsRepository Polls { get; }

        public IAnswersRepository Answers { get;  }

        public IQuestionsRepository Questions { get; }

        public UnitOfWork()
        {
            _context = new PollsContext();

            Polls = new PollsRepository(_context);
            Answers = new AnswersRepository(_context);
            Questions = new QuestionsRepository(_context);
        }

        public void Complete()
        {
            _context.Commit();
        }
    }
}
