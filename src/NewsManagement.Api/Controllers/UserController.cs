using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsManagement.Application.Features.User.Commands;
using NewsManagement.Domain.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace NewsManagement.Api.Controllers
{
    [Route("api/v1/User")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IResult> Login([Required]UserLogin userLogin)
        {
            return await _mediator.Send(new LoginCommand.Request(userLogin));
        }
    }
}
