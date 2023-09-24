using MediatR;
using NewsManagement.Application.Interfaces.Repositories;

namespace NewsManagement.Application.Features.News.Commands
{
    public abstract class SubscribeCommand
    {
        public class Request : IRequest<string> { }

        public class Handler : IRequestHandler<Request, string>
        {
            private readonly INewsRepository _repository;
            public Handler(INewsRepository repository)
            {
                _repository = repository;
            }

            public Task<string> Handle(Request request, CancellationToken cancellationToken) =>
                _repository.Subscribe();
        }
    }
}
