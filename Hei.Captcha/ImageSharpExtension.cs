using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace Hei.Captcha
{
    public static class ImageSharpExtension
    {
        /// <summary>
        /// 绘制中文字符（可以绘制字母数字，但样式可能需要改）
        /// </summary>
        /// <param name="processingContext"></param>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static IImageProcessingContext DrawingCnText(this IImageProcessingContext processingContext,
            int containerWidth, int containerHeight, string text, Rgba32 color, Font font)
        {
            var currentContext = processingContext;
            if (string.IsNullOrEmpty(text) == false)
            {
                var imgSize = currentContext.GetCurrentSize();
                var random = new Random();
                var textWidth = (imgSize.Width / text.Length);
                var img2Size = Math.Min(textWidth, imgSize.Height);
                var fontMiniSize = (int)(img2Size * 0.6);
                var fontMaxSize = (int)(img2Size * 0.95);
                for (var i = 0; i < text.Length; i++)
                {
                    using var img = new Image<Rgba32>(img2Size, img2Size);
                    var scaledFont = new Font(font, random.Next(fontMiniSize, fontMaxSize));
                    var point = new Point(textWidth * i, (containerHeight - img.Height) / 2);
                    var textOptions = new TextOptions(scaledFont)
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                    };
                    var textChar = text[i].ToString();
                    img.Mutate(c => c
                        .DrawText(textOptions, textChar, color)
                        .Rotate(random.Next(-45, 45)));
                    currentContext = currentContext.DrawImage(img, point, 1);
                    // img2.Mutate(ctx => ctx
                    //     .DrawText(textGraphicsOptions, text[i].ToString(), scaledFont, color, new Point(0, 0))
                    //     .Rotate(random.Next(-45, 45))
                    // );
                    //img.Mutate(ctx => ctx.DrawImage(img2, point, 1));
                }
            }

            return currentContext;

            // return processingContext.Apply(img =>
            // {
            //     if (string.IsNullOrEmpty(text) == false)
            //     {
            //         Random random = new Random();
            //         var textWidth = (img.Width / text.Length);
            //         var img2Size = Math.Min(textWidth, img.Height);
            //         var fontMiniSize = (int)(img2Size * 0.6);
            //         var fontMaxSize = (int)(img2Size * 0.95);
            //
            //         for (int i = 0; i < text.Length; i++)
            //         {
            //             using (Image<Rgba32> img2 = new Image<Rgba32>(img2Size, img2Size))
            //             {
            //                 Font scaledFont = new Font(font, random.Next(fontMiniSize, fontMaxSize));
            //                 var point = new Point(i * textWidth, (containerHeight - img2.Height) / 2);
            //                 var textGraphicsOptions = new TextGraphicsOptions(true)
            //                 {
            //                     HorizontalAlignment = HorizontalAlignment.Left,
            //                     VerticalAlignment = VerticalAlignment.Top
            //                 };
            //
            //                 img2.Mutate(ctx => ctx
            //                     .DrawText(textGraphicsOptions, text[i].ToString(), scaledFont, color, new Point(0, 0))
            //                     .Rotate(random.Next(-45, 45))
            //                 );
            //                 img.Mutate(ctx => ctx.DrawImage(img2, point, 1));
            //             }
            //         }
            //     }
            // });
        }

        public static IImageProcessingContext DrawingEnText(this IImageProcessingContext processingContext,
            int containerWidth, int containerHeight, string text, string[] colorHexArr, Font[] fonts)
        {
            var currentContext = processingContext;

            if (string.IsNullOrEmpty(text) == false)
            {
                var imgSize = currentContext.GetCurrentSize();
                var random = new Random();
                var textWidth = (imgSize.Width / text.Length);
                var img2Size = Math.Min(textWidth, imgSize.Height);
                var fontMiniSize = (int)(img2Size * 0.9);
                var fontMaxSize = (int)(img2Size * 1.37);
                var fontStyleArr = Enum.GetValues(typeof(FontStyle));

                for (var i = 0; i < text.Length; i++)
                {
                    using var img = new Image<Rgba32>(img2Size, img2Size);
                    var scaledFont = new Font(fonts[random.Next(0, fonts.Length)],
                        random.Next(fontMiniSize, fontMaxSize),
                        (FontStyle)fontStyleArr.GetValue(random.Next(fontStyleArr.Length)));
                    var point = new Point(i * textWidth, (containerHeight - img.Height) / 2);
                    var colorHex = colorHexArr[random.Next(0, colorHexArr.Length)];
                    var textOptions = new TextOptions(scaledFont)
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    var textChar = text[i].ToString();
                    var color = Rgba32.ParseHex(colorHex);
                    img.Mutate(ctx => ctx
                        .DrawText(textOptions, textChar, color)
                        .DrawingGrid(containerWidth, containerHeight, color, 6, 1)
                        .Rotate(random.Next(-45, 45))
                    );
                    currentContext = currentContext.DrawImage(img, point, 1);
                }
            }

            return currentContext;
        }

        /// <summary>
        /// 画圆圈（泡泡）
        /// </summary>
        /// <param name="processingContext"></param>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="count"></param>
        /// <param name="miniR"></param>
        /// <param name="maxR"></param>
        /// <param name="color"></param>
        /// <param name="canOverlap"></param>
        /// <returns></returns>
        public static IImageProcessingContext DrawingCircles(this IImageProcessingContext processingContext,
            int containerWidth, int containerHeight, int count, int miniR, int maxR, Color color,
            bool canOverlap = false)
        {
            var currentContext = processingContext;

            if (count > 0)
            {
                var random = new Random();
                var points = new List<PointF>();

                for (var i = 0; i < count; i++)
                {
                    var tempPoint = canOverlap
                        ? new PointF(random.Next(0, containerWidth), random.Next(0, containerHeight))
                        : GetCirclePointF(containerWidth, containerHeight, (miniR + maxR), ref points);

                    var ep = new EllipsePolygon(tempPoint, random.Next(miniR, maxR));

                    currentContext = currentContext
                        .Draw(color, (float)(random.Next(94, 145) / 100.0), ep.Clip());
                }
            }

            return currentContext;
        }

        /// <summary>
        /// 画杂线
        /// </summary>
        /// <param name="processingContext"></param>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="color"></param>
        /// <param name="count"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static IImageProcessingContext DrawingGrid(
            this IImageProcessingContext processingContext, int containerWidth, int containerHeight,
            Color color, int count, float thickness)
        {
            var points = new List<PointF> { new PointF(0, 0) };
            for (var i = 0; i < count; i++)
            {
                GetCirclePointF(containerWidth, containerHeight, 9, ref points);
            }

            points.Add(new PointF(containerWidth, containerHeight));
            return processingContext
                .DrawLines(color, thickness, points.ToArray());
        }

        /// <summary>
        /// 散 随机点
        /// </summary>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="lapR"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static PointF GetCirclePointF(int containerWidth, int containerHeight, double lapR,
            ref List<PointF> list)
        {
            var random = new Random();
            var newPoint = new PointF();
            var retryTimes = 10;

            do
            {
                newPoint.X = random.Next(0, containerWidth);
                newPoint.Y = random.Next(0, containerHeight);
                var tooClose = false;
                foreach (var p in list)
                {
                    var tempDistance = Math.Sqrt((Math.Pow((p.X - newPoint.X), 2) + Math.Pow((p.Y - newPoint.Y), 2)));
                    if (tempDistance < lapR)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose == false)
                {
                    list.Add(newPoint);
                    break;
                }
            } while (retryTimes-- > 0);

            if (retryTimes <= 0)
            {
                list.Add(newPoint);
            }

            return newPoint;
        }
    }
}