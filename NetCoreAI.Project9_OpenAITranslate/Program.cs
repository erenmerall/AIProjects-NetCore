using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Lütfen çevirmek istediğiniz cümleyi giriniz: ");
        string inputText = Console.ReadLine();

        string apiKey = "api-key";

        string translatedText = await TranslateTextToEnglish(inputText, apiKey);

        if (!string.IsNullOrEmpty(translatedText))
        {
            Console.WriteLine();
            Console.WriteLine($"Çeviri (İngilizce): {translatedText}");
        }
        else
        {
            Console.WriteLine("Beklenmeyen bir hata oluştu.");
        }
    }

    private static async Task<string> TranslateTextToEnglish(string text, string apiKey)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a translator that translates any language to English clearly and naturally." },
                    new { role = "user", content = $"Translate this text to English: {text}" }
                }
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("API Hatası:");
                    Console.WriteLine(responseString);
                    return null;
                }

                dynamic responseObject = JsonConvert.DeserializeObject(responseString);
                string translation = responseObject?.choices?[0]?.message?.content?.ToString();
                return translation?.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                return null;
            }
        }
    }
}
