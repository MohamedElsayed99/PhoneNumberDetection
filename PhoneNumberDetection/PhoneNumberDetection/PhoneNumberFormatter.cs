namespace PhoneNumberDetection.PhoneNumberDetection
{
    public class PhoneNumberFormatter
    {
        public static string Format(List<PhoneNumberInfo> phoneNumbers)
        {
            return string.Join("\n", phoneNumbers.Select(p => $"Detected: {p.OriginalText} (Normalized: {p.NormalizedNumber}, Format: {p.Format})"));
        }
    }

}

