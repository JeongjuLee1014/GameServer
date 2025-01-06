using Microsoft.AspNetCore.Mvc;

namespace kakaoTemp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
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
    }
}