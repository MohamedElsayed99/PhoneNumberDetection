
using Microsoft.Extensions.DependencyInjection;
using PhoneNumberDetection.Interfaces;
using System;
using System.IO;
using System.Text;

namespace PhoneNumberDetection.PhoneNumberDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

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

/*        static string GetInputFromFile()
        {
            while (true)
            {
                Console.WriteLine("Enter file path:");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    try
                    {
                        return File.ReadAllText(filePath);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Access to the file is denied. Please check file permissions.");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("The file was not found. Please check the file path.");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"An I/O error occurred: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("File does not exist. Please enter a valid file path.");
                }
            }
        }
*/    }
}



