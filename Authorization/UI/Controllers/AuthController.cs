using Authorization.Application.Responses;
using Authorization.Application.UseCases.Commands.Create;
using Authorization.Application.UseCases.Commands.Delete;
using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<CustomSucessResponce<List<GetUserDto>>>> GetUsers()
    {
        var result =await _mediator.Send(new GetUsersRequest());
        //if (result.Count < 1)
        //{
        //    return NoContent();
        //}
        return Ok(new CustomSucessResponce<List<GetUserDto>>(result));
    }

    [HttpPost]
    public async Task<ActionResult<CustomSucessResponce<GetUserDto>>> GetUserInfo()
    {
        var result = await _mediator.Send(new GetUserRequest());
        return Ok(new CustomSucessResponce<GetUserDto>(result));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/api/register")]
    public async Task<ActionResult<BaseSucessResponse>> RegisterUser([FromBody] RegistrationEntity registrationEntity)
    {
        await _mediator.Send(new RegisterUserRequest{Entity = registrationEntity});
        return Ok(new BaseSucessResponse());
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/api/login")]
    public async Task<ActionResult<BaseSucessResponse>> LoginUser([FromBody] LoginEntity loginEntity)
    {
        //_logger.LogInformation("Logging in....");
        await _mediator.Send(new LoginUserRequest{Entity = loginEntity});
        return Ok(new BaseSucessResponse());
    }

    [HttpPut]
    public async Task<ActionResult<BaseSucessResponse>> ChangeUserData([FromBody] RegistrationEntity registrationEntity)
    {
        await _mediator.Send(new PutUserRequest{Entity = registrationEntity});
        return Ok(new BaseSucessResponse());
    }

    [HttpDelete]
    public async Task<ActionResult<BaseSucessResponse>> DeleteUser()
    {
        await _mediator.Send(new DeleteUserRequest()); 
        return Ok(new BaseSucessResponse());
    }
}