using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrzepisakApi.api.src.Features.Auth.Application.Register;
using PrzepisakApi.src.Features.Auth.Application.DTOs;

namespace PrzepisakApi.src.Features.Auth.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
       // private readonly ITokenService _tokenService;
        public AuthController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationDTO)
        {
            var command = _mapper.Map<RegisterCommand>(registrationDTO);
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        //[HttpPost("login")]
        //public async Task<IActionResult> Login(Login login)
        //{
        //    var command = _mapper.Map<LoginCommand>(login);
        //    var response = await _mediator.Send(command);

        //    if (!response.IsSuccess)
        //        return BadRequest(response.Errors);

        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict,
        //        Expires = DateTime.UtcNow.AddDays(7)
        //    };

        //    Response.Cookies.Append("refreshToken", response.Value.RefreshToken, cookieOptions);

        //    return Ok(new AuthenticationResult
        //    {
        //        Token = response.Value.Token,
        //        Expiration = response.Value.Expiration
        //    });
        //}

        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    if (refreshToken == null)
        //        return Unauthorized("Refresh token missing");

        //    var accessToken = Request.Headers["Authorization"]
        //        .ToString()
        //        .Replace("Bearer ", "");

        //    var handler = new JwtSecurityTokenHandler();
        //    var jwt = handler.ReadJwtToken(accessToken);

        //    var identityId = jwt.Claims.First(x => x.Type == "IdentityId").Value;

        //    var tokens = await _tokenService.RefreshTokensAsync(identityId, refreshToken);
        //    if (tokens == null)
        //        return Unauthorized("Invalid refresh token");

        //    Response.Cookies.Append(
        //        "refreshToken",
        //        tokens.RefreshToken,
        //        new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Secure = true,
        //            SameSite = SameSiteMode.Strict,
        //            Expires = DateTime.UtcNow.AddDays(7)
        //        }
        //    );

        //    return Ok(new
        //    {
        //        token = tokens.AccessToken,
        //        expiration = DateTime.UtcNow.AddHours(2)
        //    });
        }
    }
