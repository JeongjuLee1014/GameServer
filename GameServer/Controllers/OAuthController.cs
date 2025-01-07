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

        // 로그인 플랫폼에 대한 상수 정의
        public enum Platform
        {
            Kakao = 1,
            Google,
            Naver
        }

        public OAuthController(OAuthService oauthService, GameContext gameContext)
        {
            _oauthService = oauthService;
            _gameContext = gameContext;
        }

        public async Task<IActionResult> ReturnLoginCompleteResponse()
        {
            var loginCompleteResponse = await System.IO.File.ReadAllTextAsync("./loginCompletePage.html");
            return Content(loginCompleteResponse, "text/html");
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

        [HttpGet("kakao")]
        public async Task<IActionResult> HandleKakaoOAuthRedirect([FromQuery] string code)
        {
            string accessToken = await _oauthService.RequestAccessTokenFromKakao(code);
            string userId = await _oauthService.RequestUserIdFromKakao(accessToken);
            userId = ((int)(Platform.Kakao)).ToString() + userId.ToString();

            await ProcessUserLogin(userId);

            return await ReturnLoginCompleteResponse();
        }

        [HttpGet("naver")]
        public async Task<IActionResult> HandleNaverOAuthRedirect([FromQuery] string code)
        {
            string accessToken = await _oauthService.RequestAccessTokenFromNaver(code);
            string userId = await _oauthService.RequestUserIdFromNaver(accessToken);
            userId = (((int)(Platform.Naver)).ToString() + userId.ToString());

            await ProcessUserLogin(userId);

            return await ReturnLoginCompleteResponse();
        }

        public async Task<bool> isJoined(string userId)
        {
            return await _gameContext.Users.AnyAsync(user => user.Id == userId);
        }

        public async Task<IActionResult> Login(string userId, string sessionId)
        {
            // user의 session_id 값을 업데이트
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

        public async Task Join(string userId, string session_id)
        {
            await _gameContext.Users.AddAsync(new User
            {
                Id = userId,
                NickName = "",
                SessionId = session_id
            });

            await _gameContext.SaveChangesAsync();
        }

        private bool UserExists(string id)
        {
            return _gameContext.Users.Any(e => e.Id == id);
        }
    }
}