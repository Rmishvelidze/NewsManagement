using AutoMapper;
using Shared.Library;
using Shared.Mediator;
using NewsManagement.Application.Errors;
using NewsManagement.Domain.Interfaces;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodo
{
    public class GetTodoQueryHandler : IQueryHandler<GetTodoQuery, GetTodoDto>
    {
        private readonly ITodoQueryRepository _todoQueryRepository;
        private readonly IMapper _mapper;

        public GetTodoQueryHandler(
            ITodoQueryRepository todoQueryRepository,
            IMapper mapper
        )
        {
            _todoQueryRepository = todoQueryRepository;
            _mapper = mapper;
        }

        public async Task<Result<GetTodoDto>> Handle(
            GetTodoQuery request,
            CancellationToken cancellationToken
        ) =>
            Result
                .Create(await _todoQueryRepository.Get(request.Id, cancellationToken))
                .Ensure(result => result is not null, DomainErrors.Todo.NotFound(request.Id))
                .Map(_mapper.Map<GetTodoDto>);
    }
}