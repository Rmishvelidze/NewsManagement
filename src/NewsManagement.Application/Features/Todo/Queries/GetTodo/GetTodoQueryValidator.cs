using FluentValidation;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodo
{
    public class GetTodoQueryValidator : AbstractValidator<GetTodoQuery>
    {
        public GetTodoQueryValidator()
        {
        }
    }
}