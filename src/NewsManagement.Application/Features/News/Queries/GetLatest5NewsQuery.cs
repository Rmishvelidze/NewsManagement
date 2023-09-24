using MediatR;
using NewsManagement.Application.Interfaces.Repositories;
using NewsManagement.Domain.DTOs;

namespace NewsManagement.Application.Features.News.Queries
{
    public abstract class GetLatest5NewsQuery
    {
        public class Request : IRequest<List<NewsDto>?> { }

        public class Handler : IRequestHandler<Request, List<NewsDto>?>
        {
            private readonly INewsRepository _repository;

            public Handler(INewsRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<NewsDto>?> Handle(Request request, CancellationToken cancellationToken)
            {
                var newsDtos = _repository.GetLatest5News()?.Result;
                return await Task.FromResult(newsDtos);
            }
        }
    }
}
