using Microsoft.AspNetCore.Mvc;
using GameServer;
using System.Diagnostics;

namespace kakaoTemp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // RedirectTo*Login process client's login request
        // 1. set session id
        // 2. create url for redirection
        // 3. return redirection response

        [HttpGet("kakao")]
        public IActionResult RedirectToKakaoLogin([FromQuery] string? sessionId)
        {
            var validationResult = HandleLogin(sessionId);
            if (validationResult != null)
            {
                return validationResult; // sessionID가 유효하지 않은 경우 처리
            }

            HttpContext.Session.SetString("SessionId", sessionId);

            const string clientId = ApiConstants.KAKAO_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/kakao";
            const string responseType = "code";

            const string redirectUrl = "https://kauth.kakao.com/oauth/authorize" +
                "?client_id=" + clientId +
                "&redirect_uri=" + redirectUri +
                "&response_type=" + responseType;

            return Redirect(redirectUrl);
        }

        [HttpGet("naver")]
        public IActionResult RedirectToNaverLogin([FromQuery] string? sessionId)
        {
            var validationResult = HandleLogin(sessionId);
            if (validationResult != null)
            {
                return validationResult; // sessionID가 유효하지 않은 경우 처리
            }

            HttpContext.Session.SetString("SessionId", sessionId);

            const string clientId = ApiConstants.NAVER_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/naver";
            const string responseType = "code";
            const string state = "1234";

            const string redirectUrl = "https://nid.naver.com/oauth2.0/authorize" +
                "?response_type=" + responseType +
                "&client_id=" + clientId +
                "&redirect_uri=" + redirectUri +
                "&state=" + state;

            return Redirect(redirectUrl);
        }

        [HttpGet("google")]
        public IActionResult RedirectToGoogleLogin([FromQuery] string? sessionId)
        {
            var validationResult = HandleLogin(sessionId);
            if (validationResult != null)
            {
                return validationResult; // sessionID가 유효하지 않은 경우 처리
            }

            HttpContext.Session.SetString("SessionId", sessionId);

            const string clientId = ApiConstants.GOOGLE_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/google";
            const string responseType = "code";
            const string scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

            const string redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
                "?client_id=" + clientId +
                "&redirect_uri=" + redirectUri +
                "&response_type=" + responseType +
                "&scope=" + scope;

            return Redirect(redirectUrl);
        }

        // sessionID가 null인지 아닌지를 판별하여
        // null이면 로그인을 재시도하라는 페이지를
        // null이 아니면 정상작동하라는 반환을 하는 메서드
        private IActionResult HandleLogin(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                try
                {
                    var retryLoginPath = Path.Combine(Directory.GetCurrentDirectory(), "ResponsePages/retryLoginPage.html");
                    var fileContent = System.IO.File.ReadAllText(retryLoginPath);
                    return Content(fileContent, "text/html");

                }
                catch (FileNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 재시도 필요. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (DirectoryNotFoundException)
                {
                    var errorResponse = "<html><body><h1>로그인 재시도 필요. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
                catch (Exception ex)
                {
                    var errorResponse = $"<html><body><h1>로그인 재시도 필요. 앱으로 돌아가세요.</h1></body></html>";
                    return Content(errorResponse, "text/html");
                }
            }

            return null;
        }
    }
}