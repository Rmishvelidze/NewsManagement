using AutoMapper;
using Shared.Library;
using Shared.Mediator;
using NewsManagement.Domain.Interfaces;

namespace NewsManagement.Application.Features.Todo.Commands.CreateTodo
{
    public class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand, Guid>
    {
        private readonly ITodoQueryRepository _todoQueryRepository;
        private readonly IMapper _mapper;

        public CreateTodoCommandHandler(
            ITodoQueryRepository todoQueryRepository,
            IMapper mapper
        )
        {
            _todoQueryRepository = todoQueryRepository;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = _mapper.Map<Domain.Models.Todo>(request);
            todo.Id = Guid.NewGuid();

            return Result.Success(await _todoQueryRepository.Create(todo, cancellationToken));
        }
    }
}