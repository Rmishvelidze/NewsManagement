using Shared.Mediator;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodo
{
    public record GetTodoQuery(Guid Id) : IQuery<GetTodoDto>;
}