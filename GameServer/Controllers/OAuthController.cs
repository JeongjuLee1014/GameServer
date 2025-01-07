using GameServer.Models;
using GameServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly OAuthService _oauthService;
        private readonly GameContext _gameContext;

        public OAuthController(OAuthService oauthService, GameContext gameContext)
        {
            _oauthService = oauthService;
            _gameContext = gameContext;
        }

        [HttpGet("kakao")]
        public async Task<IActionResult> HandleKakaoOAuthRedirect([FromQuery] string code)
        {
            string accessToken = await _oauthService.RequestAccessTokenFromKakao(code);
            string userId = await _oauthService.RequestUserIdFromKakao(accessToken);
            
            await ProcessUserLogin(userId);
            
            return await ReturnLoginCompleteResponse();
        }

        [HttpGet("google")]
        public async Task<IActionResult> HandleGoogleOAuthRedirect([FromQuery] string code)
        {
            string access_token = await _oauthService.RequestAccessTokenFromGoogle(code);
            string userId = await _oauthService.RequestUserIdFromGoogle(access_token);
            
            await ProcessUserLogin(userId);
            
            return await ReturnLoginCompleteResponse();
        }

        [HttpGet("naver")]
        public async Task<IActionResult> HandleNaverOAuthRedirect([FromQuery] string code)
        {
            string access_token = await _oauthService.RequestAccessTokenFromNaver(code);
            string userId = await _oauthService.RequestUserIdFromNaver(access_token);
            
            await ProcessUserLogin(userId);
            
            return await ReturnLoginCompleteResponse();
        }

        public async Task ProcessUserLogin(string userId)
        {
            string sessionId = HttpContext.Session.GetString("SessionId")!;

            if (await isJoined(userId))
            {
                await Login(userId, sessionId);
            }
            else
            {
                await Join(userId, sessionId);
            }
        }

        public async Task<IActionResult> ReturnLoginCompleteResponse()
        {
            var loginCompleteResponse = await System.IO.File.ReadAllTextAsync("./loginCompletePage.html");
            return Content(loginCompleteResponse, "text/html");
        }

        public async Task<bool> isJoined(string userId)
        {
            return await _gameContext.Users.AnyAsync(user => user.Id == userId);
        }

        public async Task<IActionResult> Login(string userId, string sessionId)
        {
            var user = await _gameContext.Users.FindAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            user.SessionId = sessionId;

            _gameContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _gameContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task Join(string userId, string sessionId)
        {
            await _gameContext.Users.AddAsync(new User
            {
                Id = userId,
                NickName = "",
                SessionId = sessionId
            });

            await _gameContext.SaveChangesAsync();
        }

        private bool UserExists(string id)
        {
            return _gameContext.Users.Any(e => e.Id == id);
        }
    }
}