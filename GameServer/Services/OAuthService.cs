using System.Text.Json;
using dotenv.net;

namespace GameServer.Services
{
    public class OAuthService
    {
        private readonly HttpClient _httpClient;
        //private readonly string _googleClientId;
        //private readonly string _googleClientSecret;

        //public OAuthService(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;


        //    // 환경 변수에서 Client ID, Secret 읽기
        //    _googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")!;
        //    _googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")!;

        //    if (string.IsNullOrEmpty(_googleClientId) || string.IsNullOrEmpty(_googleClientSecret))
        //    {
        //        throw new Exception("Google OAuth 환경 변수가 설정되지 않았습니다.");
        //    }
        //}

        public async Task<string> RequestAccessTokenFromKakao(string authorization_code)
        {
            const string grant_type = "authorization_code";
            const string client_id = ApiConstants.KAKAO_APP_KEY;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/kakao";

            string url = "https://kauth.kakao.com/oauth/token";

            var payload = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", grant_type),
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                new KeyValuePair<string, string>("code", authorization_code),
            };

            var content = new FormUrlEncodedContent(payload);

            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            if (responseJson != null && responseJson.TryGetValue("access_token", out var accessTokenElement))
            {
                return accessTokenElement.GetString()!;
            }

            throw new Exception("Access token not found in response");
        }


        public async Task<string> RequestUserIdFromKakao(string access_token)
        {
            string url = "https://kapi.kakao.com/v2/user/me";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            if (responseJson != null && responseJson.TryGetValue("id", out var idElement))
            {
                return idElement.GetInt64().ToString();
            }

            throw new Exception("Id not found in response");
        }

        public async Task<string> RequestAccessTokenFromGoogle(string authorization_code)
        {
            const string grant_type = "authorization_code";
            const string client_id = ApiConstants.GOOGLE_APP_KEY;
            const string client_secret = ApiConstants.GOOGLE_CLIENT_SECRET;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/google";

            string url = "https://oauth2.googleapis.com/token";

            // key-value 형식의 데이터
            var payload = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", grant_type),
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                new KeyValuePair<string, string>("code", authorization_code)
            };

            var content = new FormUrlEncodedContent(payload);

            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            if (responseJson != null && responseJson.TryGetValue("access_token", out var accessTokenElement))
            {
                return accessTokenElement.GetString()!;
            }

            throw new Exception("Access token not found in response");
        }

        public async Task<string> RequestUserIdFromGoogle(string access_token)
        {
            string url = "https://www.googleapis.com/userinfo/v2/me";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            if (responseJson != null && responseJson.TryGetValue("id", out var idElement))
            {
                return idElement.GetString()!;
            }

            throw new Exception("Id not found in response");
        }

        public async Task<string> RequestAccessTokenFromNaver(string authorization_code)
        {
            // AccessToken 추출
            const string grant_type = "authorization_code";
            const string client_id = ApiConstants.NAVER_APP_KEY;
            const string client_secret = ApiConstants.NAVER_CLIENT_SECRET;
            const string redirect_uri = ApiConstants.SERVER_URL + "/oauth/naver";

            string url = "https://nid.naver.com/oauth2.0/token";

            var payload = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", grant_type),
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                new KeyValuePair<string, string>("code", authorization_code),
            };

            var content = new FormUrlEncodedContent(payload);

            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            if (responseJson != null && responseJson.TryGetValue("access_token", out var accessTokenElement))
            {
                return accessTokenElement.GetString()!;
            }

            throw new Exception("Access token not found in response");
        }

        public async Task<string> RequestUserIdFromNaver(string access_token)
        {
            // UserId 추출
            string url = "https://openapi.naver.com/v1/nid/me";


            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            if (responseJson != null && responseJson.TryGetValue("response", out var responseElement))
            {
                var responseDetails = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseElement.GetRawText());
                if (responseDetails != null && responseDetails.TryGetValue("id", out var idElement))
                {
                    return idElement.GetString()!;
                }

            }

            throw new Exception("Id not found in response");
        }
    }
}