using System.Speech.Synthesis;

class Program
{
    static void Main(string[] args)
    {
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        synthesizer.Volume = 100;  // Set volume to maximum 
        synthesizer.Rate = 0;      // Set speaking rate to normal (konuşma hızı)

        Console.Write("Enter text to convert to speech:");
        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            synthesizer.Speak(input);
        }

        Console.ReadLine();
    }
}