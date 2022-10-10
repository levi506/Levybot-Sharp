using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using ImageMagick;

namespace LevyBotSharp.Utility
{
    public static class FileUtil
    {
        public static Regex ImageUrl = new Regex(".*\u002E{1}((png)|(gif)|(mp4)|(jpe?g)|(bmp)|(webp)|(webm)){1}");
        public static Regex GifDetect = new Regex(".*\u002E{1}(gif){1}");
        public static Regex VidDetect = new Regex(".*\u002E{1}((mp4)|(webm)){1}");


        public static MemoryStream EffResizeImage(string imageUrl, int width, int height)
        {
            var image = GetBitmap(imageUrl);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            var result = new MemoryStream();
            destImage.Save(result,ImageFormat.Png);
            destImage.Dispose();
            return result;
        }

        public static MemoryStream ResizeImage(string imageUrl, int height, int width)
        {
            var image = GetImage(imageUrl);
            image.Resize(height, width);
            MemoryStream result = new MemoryStream();
            image.Write(result);
            image.Dispose();
            return result;

        }

        public static MemoryStream ResizeGif(string imageUrl, int height, int width)
        {
            var gif = GetGif(imageUrl);
            gif.Coalesce();
            foreach (MagickImage image in gif)
            {
              image.Resize(height, width);
            }

            MemoryStream result = new MemoryStream();
            gif.Write(result);
            gif.Dispose();
            return result;
        }


        public static Bitmap GetBitmap(string url)
        {

            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            var bitmap = new Bitmap(stream);
            return bitmap;

        }

        public static MagickImage GetImage(string url)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            var img = new MagickImage(stream);
            return img;
        }

        public static MagickImageCollection GetGif(string url)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            var gif = new MagickImageCollection(stream);
            return gif;
        }

        public static bool LinkIsGraphic(string url)
        {
            return ImageUrl.IsMatch(url);
        }

        public static bool LinkIsImage(string url)
        { 
            return ImageUrl.IsMatch(url) && !VidDetect.IsMatch(url);
        }

        public static bool LinkIsGif(string url)
        {
            return GifDetect.IsMatch(url);
        }

        public static MemoryStream GetFile(string url)
        {
            using (var client = new WebClient())
            {
                var download = client.OpenRead(url);
                var File = new MemoryStream();
                download.CopyTo(File);
                File.Position = 0;
                return File;
            }
        }
    }
}
