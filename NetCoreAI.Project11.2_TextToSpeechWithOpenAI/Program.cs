using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using NAudio.Wave;

class Program
{
    private static readonly string apiKey = "api-key";
    static async Task Main(string[] args)
    {
        Console.Write("Metni giriniz: ");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyası oluşturuluyor...");
            await GenerateSpeech(input);

            Console.WriteLine("Ses çalınıyor...");
            using (var audioFile = new AudioFileReader("output.mp3"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(500);
                }
            }
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
                voice = "nova"
                //* alloy	Dengeli, nötr ve doğal konuşma için tasarlanmış temel ses.
                //* echo	Hafif yankı efektiyle, uzaktan konuşma havası veren ses tonu.
                //* fable	Hikaye anlatımına uygun, sıcak ve anlatıcı tarzında ses.
                //* onyx	Derin ve güçlü tonlarla, daha ciddi ya da otoriter bir ses karakteri.
                //* nova	Hafif, genç ve modern hissettiren, enerjik bir ses tonu.
                //* shimmer	Hafif havada titreşimli / parlak tonlarla efektli ve karakteristik ses.


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
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}


//! bu projede seslendirilen metin yeni bir mp3 dosyası olarak kaydedilir ve varsayılan medya oynatıcı ile açılır.