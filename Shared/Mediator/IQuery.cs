using Shared.Library;
using MediatR;

namespace Shared.Mediator
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}