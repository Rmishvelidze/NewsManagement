using System.IdentityModel.Tokens.Jwt;
using MediatR;
using NewsManagement.Application.Interfaces.Repositories;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace NewsManagement.Application.Features.User.Commands
{
    public abstract class LoginCommand
    {
        public class Request : IRequest<IResult>
        {
            public Domain.Models.UserModels.UserLogin User { get; set; }

            public Request(Domain.Models.UserModels.UserLogin user) => this.User = user;
        }

        public class Handler : IRequestHandler<Request, IResult>
        {
            private readonly IUserRepository _repository;
            private readonly IConfiguration _configuration;

            public Handler(IUserRepository repository, IConfiguration configuration)
            {
                _repository = repository;
                _configuration = configuration;
            }
            public async Task<IResult> Handle(Request request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.User.Username) ||
                    string.IsNullOrEmpty(request.User.Password)) return Results.BadRequest("Invalid user credentials");
                var loggedInUser = _repository.Get(request.User);
                if (loggedInUser is null) return Results.NotFound("User not found");


                var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                        new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
                        new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
                        new Claim(ClaimTypes.Surname, loggedInUser.Surname),
                        new Claim(ClaimTypes.Role, loggedInUser.Role)
                    };

                var token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)),
                        SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Results.Ok(tokenString);
            }
        }
    }
}
