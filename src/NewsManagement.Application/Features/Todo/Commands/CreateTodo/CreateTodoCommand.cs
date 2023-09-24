using Shared.Mediator;

namespace NewsManagement.Application.Features.Todo.Commands.CreateTodo
{
    public record CreateTodoCommand(string Name) : ICommand<Guid>;
}