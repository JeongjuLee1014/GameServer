using Microsoft.AspNetCore.Mvc;
using GameServer;

namespace kakaoTemp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IActionResult RedirectTo(string url, string sessionID)
        {
            HttpContext.Session.SetString("SessionId", sessionID);
            return Redirect(url);
        }

        [HttpGet("kakao")]
        public IActionResult RedirectToKakaoLogin([FromQuery] string sessionID)
        {
            const string clientId = ApiConstants.KAKAO_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/kakao";
            const string responseType = "code";

            const string redirectUrl = "https://kauth.kakao.com/oauth/authorize" +
                "?client_id=" + clientId +
                "&redirect_uri=" + redirectUri +
                "&response_type=" + responseType;

            return RedirectTo(redirectUrl, sessionID);
        }

        [HttpGet("naver")]
        public IActionResult RedirectToNaverLogin([FromQuery] string sessionID)
        {
            const string clientId = ApiConstants.NAVER_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/naver";
            const string responseType = "code";
            const string state = "1234";

            const string redirectUrl = "https://nid.naver.com/oauth2.0/authorize" +
                "?response_type=" + responseType +
                "&client_id=" + clientId +
                "&redirect_uri=" + redirectUri +
                "&state=" + state;

            return RedirectTo(redirectUrl, sessionID);
        }

        [HttpGet("google")]
        public IActionResult RedirectToGoogleLogin([FromQuery] string sessionID)
        {
            const string clientId = ApiConstants.GOOGLE_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/google";
            const string responseType = "code";
            const string scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

            const string redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
                "?client_id=" + clientId +
                "&redirect_uri=" + redirectUri +
                "&response_type=" + responseType +
                "&scope=" + scope;

            return RedirectTo(redirectUrl, sessionID);
        }
    }
}