using System;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Samuxi.WPF.Harjoitus.Utils
{
    /// <summary>
    /// Handles screenshot taking
    /// </summary>
    public static class ScreenshotUtil
    {
        private const int SCREENSHOT_WIDTH = 800;
        private const int SCREENSHOT_HEIGHT = 800; 

        public static string ScreenshotFilePath = string.Format(@"{0}\screenshot.bmp", System.Windows.Forms.Application.StartupPath);

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="element">The element to take screenshot.</param>
        /*
        public static void TakeScreenshot(UIElement element)
        {
            string filePath = ScreenshotFilePath;

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (IOException)
                {
                    filePath = string.Format(@"{0}\screenshot_{1:ddMMyyyyHHmmss}.bmp",
                        System.Windows.Forms.Application.StartupPath, DateTime.Now);
                }
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(SCREENSHOT_WIDTH, SCREENSHOT_HEIGHT, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(element);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (Stream fileStream = File.Create(filePath))
            {
                pngImage.Save(fileStream);
            }
        }*/

        public static BitmapImage TakeScreenshot(UIElement element)
        {
            var bitmap = new BitmapImage();

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(SCREENSHOT_WIDTH, SCREENSHOT_HEIGHT, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(element);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (MemoryStream fileStream = new MemoryStream())
            {
                pngImage.Save(fileStream);
               
                bitmap.BeginInit();
                bitmap.StreamSource = fileStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            return bitmap;
        }
    }
}
