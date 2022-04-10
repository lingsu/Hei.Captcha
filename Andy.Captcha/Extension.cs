using Hei.Captcha;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Andy.Captcha;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extension
    {
        public static IServiceCollection AddAndyCaptcha(this IServiceCollection services, Action<Andy.Captcha.Options> configure = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var option = new Andy.Captcha.Options();
            if (configure != null)
            {
                configure(option);
                //services.Configure(configure);
            }


            services.AddHeiCaptcha((HeiCaptchaOptions hei) =>
            {
                hei.FontSize = option.FontSize;
                hei.FontPath = option.FontPath;
                hei.Inflection = option.Inflection;
                hei.GaussianBlur = option.GaussianBlur;
                hei.Rotate = option.Rotate;
                hei.GridThickness = option.GridThickness;
                hei.GridAlpha = option.GridAlpha;
                hei.FontSize = option.FontSize;
            });

            services.AddScoped<ICaptcha, Captcha>();
            //services.AddScoped<SecurityCodeHelper>();
            //services.AddHeiCaptcha();
            return services;
        }
    }
}
