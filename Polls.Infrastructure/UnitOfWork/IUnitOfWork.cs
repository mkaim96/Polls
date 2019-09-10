using Polls.Infrastructure.Repositories;

namespace Polls.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPollsRepository Polls { get; }
        IAnswersRepository Answers { get; }

        void Complete();
    }
}