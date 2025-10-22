using System.Text;
using System.Text.Json;

namespace NetCoreAI.Project20_RecipeSuggestionWithOpenAI.Models
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private const string OpenAiUrl = "https://api.openai.com/v1/chat/completions";
        private const string apiKey = "api-key";

        public OpenAiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }
        
        public async Task<string> GetRecipeAsync(string ingredients)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new { role = "system", content = "Sen profesyonel bir aşçısın. Cevaplarını HTML formatında oluştur. Tarifi madde madde <ul><li></li></ul> yapısında yaz." },
            new { role = "user", content = $"Elimde şu malzemeler var: {ingredients}. Ne yapabilirim?" }
        },
                temperature = 0.7
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var response = await _httpClient.PostAsync(OpenAiUrl, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"OpenAI API hata döndürdü ({(int)response.StatusCode}): {responseBody}";
            }

            try
            {
                var doc = JsonDocument.Parse(responseBody);
                if (!doc.RootElement.TryGetProperty("choices", out var choices))
                    return $"Beklenen 'choices' yanıt içinde bulunamadı. Yanıt: {responseBody}";

                var content = choices[0].GetProperty("message").GetProperty("content").GetString();
                return content ?? "Cevap alınamadı.";
            }
            catch (Exception ex)
            {
                return $"JSON parse hatası: {ex.Message}\nGelen yanıt:\n{responseBody}";
            }
        }

    }
}