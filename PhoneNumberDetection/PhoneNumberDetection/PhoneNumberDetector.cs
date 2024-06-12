using System.Text.RegularExpressions;
using PhoneNumberDetection.Interfaces;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PhoneNumberDetection.PhoneNumberDetection
{
    public class PhoneNumberDetector : IPhoneNumberDetector
    {
        private static readonly Dictionary<string, string> NumberWords = new()
        {
            { "zero", "0" }, { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" },
            { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" },
            { "०", "0" }, { "१", "1" }, { "२", "2" }, { "३", "3" }, { "४", "4" },
            { "५", "5" }, { "६", "6" }, { "७", "7" }, { "८", "8" }, { "९", "9" }
        };

        public List<PhoneNumberInfo> Detect(string input)
        {
            var detectedNumbers = new List<PhoneNumberInfo>();

            // Convert textual numbers to digits
            string normalizedInput = NormalizeTextToDigits(input);

            // Detect numeric phone numbers
            var numericPhoneNumbers = DetectNumericPhoneNumbers(normalizedInput, input);

            detectedNumbers.AddRange(numericPhoneNumbers);

            return detectedNumbers;
        }

        private string NormalizeTextToDigits(string input)
        {
            var normalized = new StringBuilder();
            var words = input.Split(new[] { ' ', '-', '.', ',', '\n', '\r', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (NumberWords.TryGetValue(word.ToLowerInvariant(), out var digit))
                {
                    normalized.Append(digit);
                }
                else
                {
                    normalized.Append(word);
                }
                //normalized.Append(" "); // Preserve spaces to separate numbers
            }
            return normalized.ToString().Trim();
        }

        private List<PhoneNumberInfo> DetectNumericPhoneNumbers(string normalizedInput, string originalInput)
        {
            var patterns = new[]
            {
                @"\b\d{10}\b", // 10-digit numbers
                @"\b(\+?\d{1,3})?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,4}\b", // Numbers with country codes or separators
                @"\b\d{1,4}[-.\s]?\(\d{1,4}\)[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,4}\b" // Numbers with parentheses for area codes
            };

            var detectedNumbers = new List<PhoneNumberInfo>();

            foreach (var pattern in patterns)
            {
                var matches = Regex.Matches(normalizedInput, pattern);
                foreach (Match match in matches)
                {
                    // Extract the corresponding original input segment
                    string originalText = ExtractOriginalText(originalInput, match.Value);

                    detectedNumbers.Add(new PhoneNumberInfo
                    {
                        OriginalText = originalText,
                        NormalizedNumber = match.Value,
                        Format = DetermineFormat(match.Value)
                    });
                }
            }

            return detectedNumbers;
        }

        private string ExtractOriginalText(string originalInput, string normalizedValue)
        {
            var regex = new Regex(Regex.Escape(normalizedValue));
            var match = regex.Match(NormalizeTextToDigits(originalInput));
            if (match.Success)
            {
                int startIndex = match.Index;
                int length = match.Length;
                return originalInput.Substring(startIndex, length);
            }

            return normalizedValue; // Fallback if no better match found
        }

        private string DetermineFormat(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+"))
                return "Country code";
            if (phoneNumber.Contains("(") && phoneNumber.Contains(")"))
                return "Parentheses";
            if (phoneNumber.Contains("-") || phoneNumber.Contains(" ") || phoneNumber.Contains("."))
                return "With separators";
            return "10-digit";
        }
    }
}
