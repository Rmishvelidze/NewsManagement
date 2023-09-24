using Shared.Abstractions;
using Shared.Extensions;
using Shared.Library;
using Shared.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsManagement.Application.Features.Todo.Commands.CreateTodo;
using NewsManagement.Application.Features.Todo.Queries.GetTodo;
using NewsManagement.Application.Features.Todo.Queries.GetTodoList;

namespace NewsManagement.Api.Controllers
{
    [Route("api/v1/todos")]
    public class TodoController : ApiController
    {
        public TodoController(ISender sender)
            : base(sender)
        {
        }

        [HttpGet("{id}", Name = "GetTodo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiServiceResponse<GetTodoDto>>> Get(
            Guid id,
            CancellationToken cancellationToken
        ) =>
            await Result
                .Create(new GetTodoQuery(id))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(
                    success => Ok(new SuccessApiServiceResponse<GetTodoDto>(success)),
                    HandleFailure<GetTodoDto>
                );

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiServiceResponse<List<GetTodoListDto>>>> GetAll(
            CancellationToken cancellationToken
        ) =>
            await Result
                .Create(new GetTodoListQuery())
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(
                    success => Ok(new SuccessApiServiceResponse<List<GetTodoListDto>>(success)),
                    HandleFailure<List<GetTodoListDto>>
                );

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiServiceResponse<Guid>>> Create(
            CreateTodoCommand todo,
            CancellationToken cancellationToken
        ) =>
            await Result
                .Create(todo)
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(
                    success => CreatedAtRoute(
                        "GetTodo",
                        new { Id = success },
                        new SuccessApiServiceResponse<Guid>(success)
                    ),
                    HandleFailure<Guid>
                );
    }
}