using FluentValidation;
using MediatR;
using NewsManagement.Application.Interfaces.Repositories;
using NewsManagement.Domain.DTOs;

namespace NewsManagement.Application.Features.News.Queries
{
    public abstract class GetNewsByDaysQuery
    {
        public class Request : IRequest<List<NewsDto>?>
        {
            public int Days { get; set; }
            public Request(int days) => this.Days = days;
        }

        public class Handler : IRequestHandler<Request, List<NewsDto>?>
        {
            private readonly INewsRepository _repository;

            public Handler(INewsRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<NewsDto>?> Handle(Request request, CancellationToken cancellationToken)
            {
                var newsDtos = _repository.GetNewsByDays(request.Days)?.Result;
                return await Task.FromResult(newsDtos);
            }
        }
    }
}
