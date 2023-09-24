using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
        [Route("GetAllNews")]
        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var newsDtos = await _mediator.Send(new GetAllNewsQuery.Request());
            return Ok(newsDtos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
        [HttpGet("GetNewsByDays")]
        public async Task<IActionResult> GetNewsByDays([Required]int days)
        {
            var newsDtos = await _mediator.Send(new GetNewsByDaysQuery.Request(days));
            return newsDtos!.Count == 0 ? NotFound() : Ok(newsDtos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
        [HttpGet("GetNewsByText")]
        public async Task<Result<List<NewsDto>?>> GetNewsByText([Required]string text) =>
            await _mediator.Send(new GetNewsByTextQuery.Request(text));

        [HttpGet("GetLatest5News")]
        public async Task<IActionResult> GetLatest5News()
        {
            var newsDtos = await _mediator.Send(new GetLatest5NewsQuery.Request());
            return newsDtos!.Count == 0 ? NotFound() : Ok(newsDtos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("Subscribe")]
        public Task<string> Subscribe() =>
            _mediator.Send(new SubscribeCommand.Request());
    }
}
