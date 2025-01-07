using Microsoft.AspNetCore.Mvc;
using dotenv.net;
using GameServer;

namespace kakaoTemp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //private readonly string _googleClientId;

        //public LoginController()
        //{

        //    // 환경 변수에서 Client ID 읽기
        //    _googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")!;

        //    if (string.IsNullOrEmpty(_googleClientId))
        //    {
        //        throw new Exception("Google OAuth 환경 변수가 설정되지 않았습니다.");
        //    }
        //}

        [HttpGet("kakao")]
        public IActionResult RedirectToKakaoLogin([FromQuery] string session_id)
        {
            HttpContext.Session.SetString("SessionId", session_id);

            const string client_id = ApiConstants.KAKAO_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/kakao";
            const string response_type = "code";

            const string redirectUrl = "https://kauth.kakao.com/oauth/authorize" +
                "?client_id=" + client_id +
                "&redirect_uri=" + redirect_uri +
                "&response_type=" + response_type;

            return Redirect(redirectUrl);
        }

        [HttpGet("naver")]
        public IActionResult RedirectToNaverLogin([FromQuery] string session_id)
        {
            HttpContext.Session.SetString("SessionId", session_id);

            const string client_id = ApiConstants.NAVER_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/naver";
            const string response_type = "code";
            const string state = "1234";

            const string redirectUrl = "https://nid.naver.com/oauth2.0/authorize"
                + "?response_type=" + response_type
                + "&client_id=" + client_id
                + "&redirect_uri=" + redirect_uri
                + "&state=" + state;

            return Redirect(redirectUrl);
        }

        [HttpGet("google")]
        public IActionResult RedirectToGoogleLogin([FromQuery] string session_id)
        {
            HttpContext.Session.SetString("SessionId", session_id);

            const string client_id = ApiConstants.GOOGLE_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/google";
            const string response_type = "code";
            const string scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

            string redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
            "?client_id=" + client_id +
            "&redirect_uri=" + redirect_uri +
            "&response_type=" + response_type +
            "&scope=" + scope;

            return Redirect(redirectUrl);
        }
    }
}