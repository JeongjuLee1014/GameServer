using System.Text.Json;

namespace GameServer.Services
{
    public class OAuthService
    {
        private readonly HttpClient _httpClient;

        public OAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RequestAccessTokenFromKakao(string authorization_code)
        {
            const string grant_type = "authorization_code";
            const string client_id = "49209eb683ce3a79ad35d14c2dc39b60";
            const string redirect_uri = "https://localhost:7032/oauth/kakao";

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



        public async Task<string> RequestAccessTokenFromNaver(string authorization_code)
        {
            // AccessToken 추출
            const string grant_type = "authorization_code";
            const string client_id = "pKaHA3PgKs1zqls08bAy";
            const string client_secret = "vSAeWrI6bB";
            const string redirect_uri = "https://localhost:7032/oauth/naver";

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