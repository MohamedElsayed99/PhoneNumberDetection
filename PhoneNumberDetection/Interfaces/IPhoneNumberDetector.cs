using PhoneNumberDetection.PhoneNumberDetection;

namespace PhoneNumberDetection.Interfaces
{
    public interface IPhoneNumberDetector
    {
        List<PhoneNumberInfo> Detect(string input);
    }

}

