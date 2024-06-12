
using Microsoft.Extensions.DependencyInjection;
using PhoneNumberDetection.Interfaces;
using System;
using System.IO;

namespace PhoneNumberDetection.PhoneNumberDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPhoneNumberDetector, PhoneNumberDetector>()
                .BuildServiceProvider();

            var detector = serviceProvider.GetService<IPhoneNumberDetector>();

            string input = GetInput();

            var phoneNumbers = detector.Detect(input);
            string formattedOutput = PhoneNumberFormatter.Format(phoneNumbers);

            Console.WriteLine("Detected Phone Numbers:");
            Console.WriteLine(formattedOutput);
        }

        static string GetInput()
        {
            Console.WriteLine("Enter '1' to input text directly or '2' to read from a file:");
            string choice = Console.ReadLine();

            if (choice == "2")
            {
                Console.WriteLine("Enter file path:");
                string filePath = Console.ReadLine();
                return File.ReadAllText(filePath);
            }
            else
            {
                Console.WriteLine("Enter text:");
                return Console.ReadLine();
            }
        }
    }


}


