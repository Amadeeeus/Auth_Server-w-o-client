using Authorization.Application.Responses;
using Authorization.Application.UseCases.Commands.Create;
using Authorization.Application.UseCases.Commands.Delete;
using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.UI.Controllers;

[Authorize]
[ApiController]
[Route("")]

public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    private readonly ICookieService _service;
    public AuthController(ILogger<AuthController> logger, IMediator mediator, ICookieService service)
    {
        _service = service;
        _mediator = mediator;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("api/auth/getusers/list")]
    public async Task<ActionResult<CustomSucessResponce<List<GetUserDto>>>> GetUsers()
    {
        var result =await _mediator.Send(new GetUsersRequest());
        //if (result.Count < 1)
        //{
        //    return NoContent();
        //}
        return Ok(new CustomSucessResponce<List<GetUserDto>>(result));
    }

    [HttpGet]
    [Route("api/auth")]
    public async Task<ActionResult<CustomSucessResponce<UserDto>>> GetUserInfo()
    {
        var result = await _mediator.Send(new GetUserRequest());
        return Ok(new CustomSucessResponce<UserDto>(result));
    }

    [HttpGet]
    [Route("api/refresh")]
    public async Task<ActionResult<BaseSucessResponse>> RefreshToken()
    {
        await _mediator.Send(new RefreshToken());
        return Ok(new BaseSucessResponse());
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
    [Route("api/auth")]
    public async Task<ActionResult<BaseSucessResponse>> ChangeUserData([FromBody] RegistrationEntity registrationEntity)
    {
        await _mediator.Send(new PutUserRequest{Entity = registrationEntity});
        return Ok(new BaseSucessResponse());
    }

    [HttpDelete]
    [Route("api/auth")]
    public async Task<ActionResult<BaseSucessResponse>> DeleteUser()
    {
        await _mediator.Send(new DeleteUserRequest()); 
        return Ok(new BaseSucessResponse());
    }
    
    [HttpGet]
    [Route("/logout")]
    public async Task<ActionResult<BaseSucessResponse>> LogoutUser()
    {
        await _service.Logout();
        return Ok(new BaseSucessResponse());
    }
}