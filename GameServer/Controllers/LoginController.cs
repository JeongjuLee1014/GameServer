using Microsoft.AspNetCore.Mvc;
using dotenv.net;

namespace kakaoTemp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _googleClientId;

        public LoginController()
        {

            // 환경 변수에서 Client ID 읽기
            _googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");

            if (string.IsNullOrEmpty(_googleClientId))
            {
                throw new Exception("Google OAuth 환경 변수가 설정되지 않았습니다.");
            }
        }

        [HttpGet("kakao")]
        public IActionResult RedirectToKakaoLogin([FromQuery] string session_id)
        {
            HttpContext.Session.SetString("SessionId", session_id);

            const string client_id = "49209eb683ce3a79ad35d14c2dc39b60";
            const string redirect_uri = "https://localhost:7032/oauth/kakao";
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

            const string client_id = "pKaHA3PgKs1zqls08bAy";
            const string redirect_uri = "https://localhost:7032/oauth/naver";
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


            const string redirect_uri = "https://localhost:7032/oauth/google";
            const string response_type = "code";
            const string scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

            string redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth" +
            "?client_id=" + _googleClientId +
            "&redirect_uri=" + redirect_uri +
            "&response_type=" + response_type +
            "&scope=" + scope;

            return Redirect(redirectUrl);
        }
    }
}