using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading.Tasks;

namespace Hei.Captcha
{
    public static class ImageRgba32Extension
    {
        public static byte[] ToPngArray<TPixel>(this Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms,PngFormat.Instance);
                return ms.ToArray();
            }
        }
        public static async Task<byte[]> ToPngArrayAsync<TPixel>(this Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var ms = new MemoryStream())
            {
                await img.SaveAsPngAsync(ms);
                //await img.SaveAsPngAsync(ms, PngFormat.Instance);
                return ms.ToArray();
            }
        }
        public static byte[] ToGifArray<TPixel>(this Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, new GifEncoder());
                return ms.ToArray();
            }
        }
        public static async Task<byte[]> ToGifArrayAsync<TPixel>(this Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var ms = new MemoryStream())
            {
                await img.SaveAsGifAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
