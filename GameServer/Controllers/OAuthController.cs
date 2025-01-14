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

        [HttpGet("kakao")]
        public async Task<IActionResult> HandleKakaoOAuthRedirect([FromQuery] string? code, [FromQuery] string? error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                // 인가 코드 받기 요청을 실패
                // 1. 사용자가 동의 화면에서 취소하여 로그인을 취소
                // 2. 모종의 에러 발생

                // 하나의 페이지로 처리하지 말고 응답 코드 분류하여 처리하는 게 좋을 듯
                try
                {
                    var loginCodeResponse = await System.IO.File.ReadAllTextAsync("./ResponsePages/failLoginPage.html");
                    return Content(loginCodeResponse, "text/html");
                }
                catch (FileNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (DirectoryNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (Exception ex)
                {
                    var errorResponse = $"<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
            }

            try
            {
                string accessToken = await _oauthService.RequestAccessTokenFromKakao(code);
                string userId = await _oauthService.RequestUserIdFromKakao(accessToken);

                await ProcessUserLogin(userId);

                return await ReturnLoginCompleteResponse();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing Kakao login.", detail = ex.Message });
            }
        }

        [HttpGet("google")]
        public async Task<IActionResult> HandleGoogleOAuthRedirect([FromQuery] string? code, [FromQuery] string? error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                // error: 오류 요청 나타내는 문자열. ex) access_denied, invalid_request, etc.
                // error_description: 오류에 대한 자세한 요청
                // state: 단순 요청 무결성 확인용
                try
                {
                    var loginCodeResponse = await System.IO.File.ReadAllTextAsync("./ResponsePages/failLoginPage.html");
                    return Content(loginCodeResponse, "text/html");
                }
                catch (FileNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (DirectoryNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (Exception ex)
                {
                    var errorResponse = $"<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
            }

            try
            {
                string access_token = await _oauthService.RequestAccessTokenFromGoogle(code);
                string userId = await _oauthService.RequestUserIdFromGoogle(access_token);

                await ProcessUserLogin(userId);

                return await ReturnLoginCompleteResponse();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing Kakao login.", detail = ex.Message });
            }
        }

        [HttpGet("naver")]
        public async Task<IActionResult> HandleNaverOAuthRedirect([FromQuery] string? code, [FromQuery] string? error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                // google과 동일
                try
                {
                    var loginCodeResponse = await System.IO.File.ReadAllTextAsync("./ResponsePages/failLoginPage.html");
                    return Content(loginCodeResponse, "text/html");
                }
                catch (FileNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (DirectoryNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (Exception ex)
                {
                    var errorResponse = $"<html><body><h1>로그인 실패. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
            }

            try
            {
                string access_token = await _oauthService.RequestAccessTokenFromNaver(code);
                string userId = await _oauthService.RequestUserIdFromNaver(access_token);

                await ProcessUserLogin(userId);

                return await ReturnLoginCompleteResponse();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing Kakao login.", detail = ex.Message });
            }
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
            try
            {
                var loginCompleteResponse = await System.IO.File.ReadAllTextAsync("./ResponsePages/loginCompletePage.html");
                return Content(loginCompleteResponse, "text/html");
            }
            catch (FileNotFoundException)
            {
                var errorResponse = "<html><body><h1>로그인 완료. 앱으로 돌아가세요.</h1></body></html>";
                return Content(errorResponse, "text/html");
            }
            catch (DirectoryNotFoundException)
            {
                var errorResponse = "<html><body><h1>로그인 완료. 앱으로 돌아가세요.</h1></body></html>";
                return Content(errorResponse, "text/html");
            }
            catch (Exception ex)
            {
                var errorResponse = $"<html><body><h1>로그인 완료. 앱으로 돌아가세요.</h1></body></html>";
                return Content(errorResponse, "text/html");
            }
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

            // 두 기기에서 같은 계정으로 로그인할 때 먼저 로그인한 기기에서 로그아웃되게
            if (!string.IsNullOrEmpty(user.SessionId))
            {
                await Logout(user.SessionId);
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


        // 해당 메서드 체크할 필요 O
        private async Task Logout(string sessionId)
        {
            // 로그아웃 처리 로직 (예: 해당 세션에 대한 추가 작업)
            var userToLogout = await _gameContext.Users.FirstOrDefaultAsync(u => u.SessionId == sessionId);

            if (userToLogout != null)
            {
                // 세션 종료 (세션 ID를 비워놓기)
                userToLogout.SessionId = null;

                // 이후 처리법

                // 데이터베이스에 변경 사항 반영
                _gameContext.Entry(userToLogout).State = EntityState.Modified;
                await _gameContext.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> Join(string userId, string sessionId)
        {
            try
            {
                await _gameContext.Users.AddAsync(new User
                {
                    Id = userId,
                    NickName = "",
                    SessionId = sessionId
                });
            }
            catch (DbUpdateException)
            {
                if (UserExists(userId))
                {
                    // 이미 존재하는 userId -> 이 경우가 있을 가능성이 거의 없음
                    return await Login(userId, sessionId);
                }
                else
                {
                    throw;
                }
            }

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

        private bool UserExists(string id)
        {
            return _gameContext.Users.Any(e => e.Id == id);
        }
    }
}