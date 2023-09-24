using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsManagement.Application.Features.News.Commands;
using NewsManagement.Application.Features.News.Queries;
using NewsManagement.Domain.DTOs;
using Shared.Library;

namespace NewsManagement.Api.Controllers
{
    [Route("api/v1/News")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        //    public NewsController(ISender sender) : base(sender) { }

        //    [HttpGet( Name = "GetNews")]
        //    [ProducesResponseType(StatusCodes.Status200OK)]
        //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //    [ProducesResponseType(StatusCodes.Status404NotFound)]
        //    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        //    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //    public async Task<ActionResult<ApiServiceResponse<NewsDto>>> Get() =>
        //        await Result
        //            .Create(new GetNewsQuery())
        //            .Bind(query => Sender.Send(query))
        //            .Match(
        //                success => Ok(new SuccessApiServiceResponse<NewsDto>(success)),
        //                HandleFailure<NewsDto>
        //            );

        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("GetAllNews")]
        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var newsDtos = await _mediator.Send(new GetAllNewsQuery.Request());
            return Ok(newsDtos);
        }

        [HttpGet("GetNewsByDays")]
        public async Task<IActionResult> GetNewsByDays([Required]int days)
        {
            var newsDtos = await _mediator.Send(new GetNewsByDaysQuery.Request(days));
            return newsDtos!.Count == 0 ? NotFound() : Ok(newsDtos);
        }

        [HttpGet("GetNewsByText")]
        public async Task<Result<List<NewsDto>?>> GetNewsByText(string text) =>
            await _mediator.Send(new GetNewsByTextQuery.Request(text));

        [HttpGet("GetLatest5News")]
        public async Task<IActionResult> GetLatest5News()
        {
            var newsDtos = await _mediator.Send(new GetLatest5NewsQuery.Request());
            return newsDtos!.Count == 0 ? NotFound() : Ok(newsDtos);
        }

        [HttpPost("Subscribe")]
        public Task<string> Subscribe() =>
            _mediator.Send(new SubscribeCommand.Request());
    }
}
