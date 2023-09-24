using NewsManagement.Domain.Models;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodo
{
    public record GetTodoDto(string Name, TodoStatus Status);
}