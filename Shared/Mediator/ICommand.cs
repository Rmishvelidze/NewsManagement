using Shared.Library;
using MediatR;

namespace Shared.Mediator
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}