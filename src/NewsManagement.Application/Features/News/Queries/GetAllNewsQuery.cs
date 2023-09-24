using AutoMapper;
using MediatR;
using NewsManagement.Application.Interfaces.Repositories;
using NewsManagement.Domain.DTOs;

namespace NewsManagement.Application.Features.News.Queries
{
    public abstract class GetAllNewsQuery
    {
        public class Request : IRequest<IEnumerable<NewsDto>> { }

        public class Handler : IRequestHandler<Request, IEnumerable<NewsDto>>
        {
            private readonly INewsRepository _repository;
            private readonly IMapper _mapper;

            public Handler(INewsRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<IEnumerable<NewsDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                var newsDtos = await _repository.GetAllNews()!;
                return _mapper.Map<IEnumerable<NewsDto>>(newsDtos);
            }
        }
    }
}
