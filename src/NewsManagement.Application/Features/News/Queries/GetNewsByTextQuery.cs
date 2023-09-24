using FluentValidation;
using MediatR;
using NewsManagement.Application.Interfaces.Repositories;
using NewsManagement.Domain.DTOs;
using Shared.Library;

namespace NewsManagement.Application.Features.News.Queries
{
    public abstract class GetNewsByTextQuery
    {
        public class Request : IRequest<Result<List<NewsDto>?>>
        {
            public string Text { get; set; }
            public Request(string text) => this.Text = text;
        }

        public class Handler : IRequestHandler<Request, Result<List<NewsDto>?>>
        {
            private readonly INewsRepository _repository;

            public Handler(INewsRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<List<NewsDto>?>> Handle(Request request, CancellationToken cancellationToken)
            {
                var validationResult = await new Validator().ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid) 
                    return Result.Failure<List<NewsDto>?>(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());

                var newsDtos = _repository.GetNewsByText(request.Text).Result;
                return await Task.FromResult(Result.Success(newsDtos));
            }
        }

        private class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Text)
                    .NotNull().WithMessage($"Please, enter search Text.");
            }
        }
    }
}
