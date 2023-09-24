using Shared.Mediator;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodoList
{
    public record GetTodoListQuery : IQuery<List<GetTodoListDto>>;
}