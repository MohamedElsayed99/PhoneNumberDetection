using Xunit;
using FluentAssertions;
using PhoneNumberDetection.Interfaces;

namespace PhoneNumberDetection.PhoneNumberDetection.Tests
{
    public class PhoneNumberDetectorTests
    {
        private readonly IPhoneNumberDetector _detector = new PhoneNumberDetector();

        [Fact]
        public void Detect_ShouldReturnPhoneNumbers_ForNumericInput()
        {
            string input = "Contact me at 123-456-7890.";
            var result = _detector.Detect(input);

            result.Should().Contain(p => p.NormalizedNumber == "1234567890");
        }

        [Fact]
        public void Detect_ShouldReturnPhoneNumbers_ForTextualInput()
        {
            string input = "Contact me at nine eight one zero zero two three four five five.";
            var result = _detector.Detect(input);

            result.Should().Contain(p => p.NormalizedNumber == "9810023455");
        }

        [Fact]
        public void Detect_ShouldHandleCountryCode()
        {
            string input = "Contact me at +91-123-456-7890.";
            var result = _detector.Detect(input);

            result.Should().Contain(p => p.NormalizedNumber == "+911234567890");
        }

        [Fact]
        public void Detect_ShouldHandleEmptyInput()
        {
            string input = "";
            var result = _detector.Detect(input);

            result.Should().BeEmpty();
        }
    }
}
