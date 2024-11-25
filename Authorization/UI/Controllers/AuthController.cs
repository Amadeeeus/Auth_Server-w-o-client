using Authorization.Application.Responses;
using Authorization.Application.UseCases.Commands.Create;
using Authorization.Application.UseCases.Commands.Delete;
using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.UI.Controllers;

[Authorize]
[ApiController]
[Route("api/auth")]

public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<CustomSucessResponce<List<UserEntity>>>> GetUsers()
    {
        List<UserEntity> result =await _mediator.Send(new GetUsersRequest());
        if (result is null)
        {
            return NoContent();
        }

        return Ok(new CustomSucessResponce<List<UserEntity>>(result));
    }

    [HttpPost]
    public async Task<ActionResult<CustomSucessResponce<UserEntity>>> GetUserInfo()
    {
        var result = await _mediator.Send(new GetUserRequest());
        return Ok(new CustomSucessResponce<UserEntity>(result));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/api/register")]
    public async Task<ActionResult<CustomSucessResponce<AuthResponceDTO>>> RegisterUser([FromBody] RegistrationEntity registrationEntity)
    {
        var result = await _mediator.Send(new RegisterUserRequest{Entity = registrationEntity});
        return Ok(new CustomSucessResponce<AuthResponceDTO>(result));
    }

    [HttpPost]
    [Route("/api/login")]
    public async Task<ActionResult<CustomSucessResponce<AuthResponceDTO>>> LoginUser([FromBody] LoginEntity loginEntity)
    {
        //_logger.LogInformation("Logging in....");
        var result = await _mediator.Send(new LoginUserRequest{Entity = loginEntity});
        return Ok(new CustomSucessResponce<AuthResponceDTO>(result));
    }

    [HttpPut]
    public async Task<ActionResult<CustomSucessResponce<AuthResponceDTO>>> ChangeUserData([FromBody] RegistrationEntity registrationEntity)
    {
        var result =await _mediator.Send(new PutUserRequest{Entity = registrationEntity});
        return Ok(new CustomSucessResponce<AuthResponceDTO>(result));
    }

    [HttpDelete]
    public async Task<ActionResult<BaseSucessResponse>> DeleteUser()
    {
        await _mediator.Send(new DeleteUserRequest()); 
        return Ok(new BaseSucessResponse());
    }
}