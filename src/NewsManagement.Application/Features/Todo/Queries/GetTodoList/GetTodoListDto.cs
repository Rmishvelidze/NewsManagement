using NewsManagement.Domain.Models;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodoList
{
    public record GetTodoListDto(string Name, TodoStatus Status);
}