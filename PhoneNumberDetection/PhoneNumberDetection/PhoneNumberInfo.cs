namespace PhoneNumberDetection.PhoneNumberDetection
{
    public class PhoneNumberInfo
    {
        public string OriginalText { get; set; } // Original detected text
        public string NormalizedNumber { get; set; } // Normalized numeric form
        public string Format { get; set; } // Format of the phone number

    }
}
