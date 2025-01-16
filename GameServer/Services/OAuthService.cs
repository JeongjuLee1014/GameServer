using System.Text.Json;

namespace GameServer.Services
{
    public class OAuthService
    {
        private readonly HttpClient _httpClient;

        public enum Platform
        {
            Kakao = 1,
            Google,
            Naver
        }

        public OAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RequestAccessTokenFromKakao(string authorizationCode)
        {
            const string grantType = "authorization_code";
            const string clientId = ApiConstants.KAKAO_APP_KEY;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/kakao";

            string url = "https://kauth.kakao.com/oauth/token";

            var payload = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("code", authorizationCode),
            };

            var content = new FormUrlEncodedContent(payload);

            // Http 예외
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Failed to get response: {e.Message}");
            }

            // Json 예외
            Dictionary<string, JsonElement>? responseJson;
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);
            }
            catch (JsonException e)
            {
                throw new Exception($"Failed to parse response JSON: {e.Message}");
            }

            // access token 추출
            if (responseJson != null && responseJson.TryGetValue("access_token", out var accessTokenElement))
            {
                return accessTokenElement.GetString()!;
            }

            throw new Exception("Access token not found in response");
        }

        public async Task<string> RequestUserIdFromKakao(string accessToken)
        {
            string url = "https://kapi.kakao.com/v2/user/me";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Failed to fetch user information from Kakao API: {e.Message}");
            }

            Dictionary<string, JsonElement>? responseJson;
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);
            }
            catch (JsonException e)
            {
                throw new Exception($"Failed to parse response JSON: {e.Message}");
            }

            if (responseJson != null && responseJson.TryGetValue("id", out var idElement))
            {
                return ((int)(Platform.Kakao)).ToString() + idElement.GetInt64().ToString();
            }

            throw new Exception("Id not found in response");
        }

        public async Task<string> RequestAccessTokenFromGoogle(string authorizationCode)
        {
            const string grantType = "authorization_code";
            const string clientId = ApiConstants.GOOGLE_APP_KEY;
            const string clientSecret = ApiConstants.GOOGLE_CLIENT_SECRET;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/google";

            string url = "https://oauth2.googleapis.com/token";

            // key-value 형식의 데이터
            var payload = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("code", authorizationCode)
            };

            var content = new FormUrlEncodedContent(payload);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Failed to get response: {e.Message}");
            }

            Dictionary<string, JsonElement>? responseJson;
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);
            }
            catch (JsonException e)
            {
                throw new Exception($"Failed to parse response JSON: {e.Message}");
            }

            if (responseJson != null && responseJson.TryGetValue("access_token", out var accessTokenElement))
            {
                return accessTokenElement.GetString()!;
            }

            throw new Exception("Access token not found in response");
        }

        public async Task<string> RequestUserIdFromGoogle(string accessToken)
        {
            string url = "https://www.googleapis.com/userinfo/v2/me";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Failed to fetch user information from Google API: {e.Message}");
            }

            Dictionary<string, JsonElement>? responseJson;
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);
            }
            catch (JsonException e)
            {
                throw new Exception($"Failed to parse response JSON: {e.Message}");
            }

            if (responseJson != null && responseJson.TryGetValue("id", out var idElement))
            {
                return ((int)(Platform.Google)).ToString() + idElement.GetString()!;
            }

            throw new Exception("Id not found in response");
        }

        public async Task<string> RequestAccessTokenFromNaver(string authorizationCode)
        {
            // AccessToken 추출
            const string grantType = "authorization_code";
            const string clientId = ApiConstants.NAVER_APP_KEY;
            const string clientSecret = ApiConstants.NAVER_CLIENT_SECRET;
            const string redirectUri = ApiConstants.SERVER_URL + "/oauth/naver";

            string url = "https://nid.naver.com/oauth2.0/token";

            var payload = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("code", authorizationCode),
            };

            var content = new FormUrlEncodedContent(payload);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Failed to get response: {e.Message}");
            }

            Dictionary<string, JsonElement>? responseJson;
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);
            }
            catch (JsonException e)
            {
                throw new Exception($"Failed to parse response JSON: {e.Message}");
            }

            if (responseJson != null && responseJson.TryGetValue("access_token", out var accessTokenElement))
            {
                return accessTokenElement.GetString()!;
            }

            throw new Exception("Access token not found in response");
        }

        public async Task<string> RequestUserIdFromNaver(string accessToken)
        {
            // UserId 추출
            string url = "https://openapi.naver.com/v1/nid/me";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Failed to fetch user information from Naver API: {e.Message}");
            }

            Dictionary<string, JsonElement>? responseJson;
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                responseJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);
            }
            catch (JsonException e)
            {
                throw new Exception($"Failed to parse response JSON: {e.Message}");
            }

            if (responseJson != null && responseJson.TryGetValue("response", out var responseElement))
            {
                var responseDetails = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseElement.GetRawText());
                if (responseDetails != null && responseDetails.TryGetValue("id", out var idElement))
                {
                    return ((int)(Platform.Naver)).ToString() + idElement.GetString()!;
                }

            }

            throw new Exception("Id not found in response");
        }
    }
}