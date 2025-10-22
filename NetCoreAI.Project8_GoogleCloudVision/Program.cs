using System.Net.Mime;
using System.Text;
﻿using Google.Cloud.Vision.V1;

class Programa
{
    static void Main(string[] args)
    {
        Console.Write("Resim yolunu girin: ");
        Console.WriteLine();
        string imagePath = Console.ReadLine();

        string credentialPath = @"path";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        try
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile(imagePath);
            var response = client.DetectText(image);
            Console.WriteLine("Resimdeki metin:");
            Console.WriteLine();
            foreach (var annotation in response)
            {
                if (!string.IsNullOrEmpty(annotation.Description))
                {
                    Console.WriteLine(annotation.Description);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata: " + ex.Message);
        }
    }
}


