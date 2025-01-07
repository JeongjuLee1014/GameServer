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
            const string client_id = ApiConstants.KAKAO_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/kakao";
            const string response_type = "code";

            const string redirectUrl = "https://kauth.kakao.com/oauth/authorize" +
                "?client_id=" + client_id +
                "&redirect_uri=" + redirect_uri +
                "&response_type=" + response_type;

            return RedirectTo(redirectUrl, sessionID);
        }

        [HttpGet("naver")]
        public IActionResult RedirectToNaverLogin([FromQuery] string sessionID)
        {
            const string client_id = ApiConstants.NAVER_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/naver";
            const string response_type = "code";
            const string state = "1234";

            const string redirectUrl = "https://nid.naver.com/oauth2.0/authorize" +
                "?response_type=" + response_type +
                "&client_id=" + client_id +
                "&redirect_uri=" + redirect_uri +
                "&state=" + state;

            return RedirectTo(redirectUrl, sessionID);
        }

        [HttpGet("google")]
        public IActionResult RedirectToGoogleLogin([FromQuery] string sessionID)
        {
            const string client_id = ApiConstants.GOOGLE_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/google";
            const string response_type = "code";
            const string scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

            string redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
                "?client_id=" + client_id +
                "&redirect_uri=" + redirect_uri +
                "&response_type=" + response_type +
                "&scope=" + scope;

            return RedirectTo(redirectUrl, sessionID);
        }
    }
}