using Hei.Captcha;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;


namespace Andy.Captcha
{
    public class Captcha : ICaptcha
    {
        const string pre = "captcha_";
        private readonly SecurityCodeHelper _securityCodeHelper;
        private readonly IMemoryCache _memoryCache;

        public Captcha(SecurityCodeHelper securityCodeHelper, IMemoryCache memoryCache)
        {
            _securityCodeHelper = securityCodeHelper;
            _memoryCache = memoryCache;
        }

        public async Task<VerificationCode> GenerateRandomEnDigitalTextAsync(int length)
        {
            var token = Guid.NewGuid().ToString();

            var code = _securityCodeHelper.GetRandomEnDigitalText(length);
            var imgbyte = await _securityCodeHelper.GetEnDigitalCodeByteAsync(code);
            var verificationCode = new VerificationCode(code, token, imgbyte);
            _memoryCache.Set(getKey(token), verificationCode, DateTimeOffset.Now.AddMinutes(10));

            return verificationCode;
        }

      
        public VerificationCode GetVerificationCode(string token)
        {
            if (_memoryCache.TryGetValue<VerificationCode>(getKey(token), out var value))
            {
                return value;
            }
            return null;
        }
        public void DeleteVerificationCode(string token)
        {
            _memoryCache.Remove(getKey(token));
        }

        private string getKey(string token)
        {
            return pre + token;
        }
    }
}
