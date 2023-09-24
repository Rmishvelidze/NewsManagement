using AutoMapper;
using Shared.Library;
using Shared.Mediator;
using NewsManagement.Application.Errors;
using NewsManagement.Domain.Interfaces;

namespace NewsManagement.Application.Features.Todo.Queries.GetTodoList
{
    public class GetTodoListQueryHandler : IQueryHandler<GetTodoListQuery, List<GetTodoListDto>>
    {
        private readonly ITodoQueryRepository _todoQueryRepository;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(
            ITodoQueryRepository todoQueryRepository,
            IMapper mapper
        )
        {
            _todoQueryRepository = todoQueryRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetTodoListDto>>> Handle(
            GetTodoListQuery request,
            CancellationToken cancellationToken
        ) =>
            Result
                .Create(await _todoQueryRepository.GetAll(cancellationToken))
                .Ensure(list => list.Count is not 0, DomainErrors.Todo.ListEmpty)
                .Map(_mapper.Map<List<GetTodoListDto>>);
    }
}