using NewsManagement.Domain.Models;

namespace NewsManagement.Domain.Interfaces
{
    public interface ITodoQueryRepository
    {
        public Task<Todo> Get(Guid id, CancellationToken cancellationToken);

        public Task<List<Todo>> GetAll(CancellationToken cancellationToken);

        public Task<Guid> Create(Todo todo, CancellationToken cancellationToken);
    }
}