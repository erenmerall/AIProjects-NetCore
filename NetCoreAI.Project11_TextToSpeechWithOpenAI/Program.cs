using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "api-key";
    static async Task Main(string[] args)
    {
        Console.Write("Metni giriniz: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyası oluşuturuluyor....");
            await GenerateSpeech(input);
            Console.Write("Ses dosyası 'output mp3' olarak kaydedildi!");
            System.Diagnostics.Process.Start("explorer.exe", "output.mp3"); //* işlemi yaptıktan sonra dosyayı açar
        }
    }

    static async Task GenerateSpeech(string text)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "tts-1",
                input = text,
                voice = "alloy"
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

            if (response.IsSuccessStatusCode)
            {
                byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync("output.mp3", audioBytes);
            }
            else
            {
                Console.WriteLine("Bir hata oluştu");
            }

        }
    }
}


//! bu projede seslendirilen metin yeni bir mp3 dosyası olarak kaydedilir ve varsayılan medya oynatıcı ile açılır.

//! 11.2 projesinde ise direkt olarak editör üzerinden seslendirme yapılır.