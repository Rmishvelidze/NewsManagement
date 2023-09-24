using Shared.Library;
using MediatR;

namespace Shared.Mediator
{
    public interface IQueryHandler<TQuery, TResponse>
        : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}