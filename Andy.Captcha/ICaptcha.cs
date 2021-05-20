using System;
using System.Threading.Tasks;

namespace Andy.Captcha
{
    public interface ICaptcha
    {
        Task<VerificationCode> GenerateRandomEnDigitalTextAsync(int length);
        VerificationCode GetVerificationCode(string token);
        void DeleteVerificationCode(string token);
    }
}
