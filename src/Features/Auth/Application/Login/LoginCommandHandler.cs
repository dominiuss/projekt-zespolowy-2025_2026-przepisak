//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using miejsce.api.src.Data;
//using miejsce.api.src.Features.Common;
//using Microsoft.EntityFrameworkCore;
//using miejsce.api.src.Features.Auth.Services;
//using miejsce.api.src.Features.Auth.Application.DTOs;

//namespace PrzepisakApi.api.src.Features.Auth.Application.Login;

//public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
//{
//    private readonly IEfContext _efContext;
//    private readonly UserManager<IdentityUser> _userManager;
//    private readonly ITokenService _tokenService;

//    public LoginCommandHandler(IEfContext efContext, UserManager<IdentityUser> userManager, ITokenService tokenService)
//    {
//        _efContext = efContext;
//        _userManager = userManager;
//        _tokenService = tokenService;
//    }

//    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _userManager.FindByEmailAsync(request.Username);
//        //if (!user.EmailConfirmed)
//        //    return Result<LoginResponse>.Failure(new List<string> { "Account not confirmed." });

//        var profile = await _efContext.UserProfiles.FirstOrDefaultAsync(up=>up.IdentityId==user.Id);

//        var tokenResponse = await _tokenService.GenerateTokensAsync(user, profile.Id);
//        var response = new LoginResponse
//        {
//            Token = tokenResponse.AccessToken,
//            RefreshToken = tokenResponse.RefreshToken,
//            Expiration = DateTime.UtcNow.AddHours(2)
//        };

//        return response;
//    }
//}
