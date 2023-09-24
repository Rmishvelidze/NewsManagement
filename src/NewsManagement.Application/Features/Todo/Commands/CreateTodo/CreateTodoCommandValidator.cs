using FluentValidation;

namespace NewsManagement.Application.Features.Todo.Commands.CreateTodo
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(element => element.Name)
                .MaximumLength(10)
                .WithErrorCode("Name is too long");
        }
    }
}