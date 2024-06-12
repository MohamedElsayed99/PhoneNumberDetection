using FluentAssertions;
using Xunit;

namespace PhoneNumberDetection
{

    namespace PhoneNumberDetection.Tests
    {
        public class PhoneNumberFormatterTests
        {
            [Fact]
            public void Format_ShouldDisplayPhoneNumberDetails()
            {
                var phoneNumbers = new List<PhoneNumberInfo>
                {
                new PhoneNumberInfo { OriginalText = "123-456-7890", NormalizedNumber = "1234567890", Format = "With separators" },
                new PhoneNumberInfo { OriginalText = "+91-123-456-7890", NormalizedNumber = "+911234567890", Format = "Country code" }
                };

                string result = PhoneNumberFormatter.Format(phoneNumbers);

                result.Should().Contain("Detected: 123-456-7890 (Normalized: 1234567890, Format: With separators)");
                result.Should().Contain("Detected: +91-123-456-7890 (Normalized: +911234567890, Format: Country code)");
            }
        }
    }

}


