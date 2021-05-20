using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.Captcha
{
    public class VerificationCode
    {
        public string Code { get; set; }
        public string Token { get; set; }
        public byte[] ImageByte { get; set; }

        public VerificationCode(string code, string token, byte[] imageByte)
        {
            Code = code;
            Token = token;
            ImageByte = imageByte;
        }
    }
}
