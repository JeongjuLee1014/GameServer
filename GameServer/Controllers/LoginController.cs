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
        public IActionResult RedirectToKakaoLogin([FromQuery] string? sessionID)
        {
            var validationResult = HandleLogin(sessionID);
            if (validationResult != null)
            {
                return validationResult; // sessionID가 유효하지 않은 경우 처리
            }

            HttpContext.Session.SetString("SessionId", sessionID);

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
        public IActionResult RedirectToNaverLogin([FromQuery] string? sessionID)
        {
            var validationResult = HandleLogin(sessionID);
            if (validationResult != null)
            {
                return validationResult; // sessionID가 유효하지 않은 경우 처리
            }

            HttpContext.Session.SetString("SessionId", sessionID);

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
        public IActionResult RedirectToGoogleLogin([FromQuery] string? sessionID)
        {
            var validationResult = HandleLogin(sessionID);
            if (validationResult != null)
            {
                return validationResult; // sessionID가 유효하지 않은 경우 처리
            }

            HttpContext.Session.SetString("SessionId", sessionID);

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

        // 
        private IActionResult HandleLogin(string sessionID)
        {
            if (string.IsNullOrEmpty(sessionID))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ResponsePages/retryLoginPage.html");
                //if (!System.IO.File.Exists(filePath))
                //{
                //    return StatusCode(500, "Retry page not found.");
                //}
                return PhysicalFile(filePath, "text/html");
            }

            return null;
        }
    }
}