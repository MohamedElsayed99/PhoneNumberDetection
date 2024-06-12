using System.Text.RegularExpressions;
using PhoneNumberDetection.Interfaces;

namespace PhoneNumberDetection.PhoneNumberDetection
{
    public class PhoneNumberDetector : IPhoneNumberDetector
    {
        private static readonly Dictionary<string, string> NumberWords = new()
        {
            { "zero", "0" }, { "one", "1" }, { "two", "2" }, { "three", "3" },
            { "four", "4" }, { "five", "5" }, { "six", "6" }, { "seven", "7" },
            { "eight", "8" }, { "nine", "9" },
            { "शून्य", "0" }, { "एक", "1" }, { "दो", "2" }, { "तीन", "3" },
            { "चार", "4" }, { "पांच", "5" }, { "छह", "6" }, { "सात", "7" },
            { "आठ", "8" }, { "नौ", "9" }
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
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return string.Join("", words.Select(word =>
            {
                string lowerWord = word.ToLower().TrimEnd('.', ',');
                return NumberWords.ContainsKey(lowerWord) ? NumberWords[lowerWord] : lowerWord;
            }));
        }

        private List<PhoneNumberInfo> DetectNumericPhoneNumbers(string normalizedInput, string originalInput)
        {
            var patterns = new[]
            {
                @"\b\d{10}\b", // 10-digit numbers
                @"\b(\+\d{1,3})?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,4}\b", // Numbers with country codes or separators
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
            // Find the part in original input that matches the normalized value
            var words = originalInput.Split(new[] { ' ', '-', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var normalizedWords = normalizedValue.ToCharArray().Select(c => c.ToString());

            foreach (var normalizedWord in normalizedWords)
            {
                foreach (var word in words)
                {
                    if (word.Contains(normalizedWord))
                    {
                        return word;
                    }
                }
            }

            return normalizedValue; // Fallback if no better match found
        }

        private string DetermineFormat(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+"))
                return "Country code";
            if (phoneNumber.Contains("(") && phoneNumber.Contains(")"))
                return "Parentheses";
            if (phoneNumber.Contains("-") || phoneNumber.Contains(" "))
                return "With separators";
            return "10-digit";
        }
    }
}





