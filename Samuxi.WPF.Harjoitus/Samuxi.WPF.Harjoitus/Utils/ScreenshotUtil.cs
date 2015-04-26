using System.IO;
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
        private const int SCREENSHOT_WIDTH = 500;
        private const int SCREENSHOT_HEIGHT = 500; 

        public static string ScreenshotFilePath = string.Format(@"{0}\screenshot.bmp", System.Windows.Forms.Application.StartupPath);

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="element">The element to take screenshot.</param>
        public static void TakeScreenshot(UIElement element)
        {
            if (File.Exists(ScreenshotFilePath))
            {
                File.Delete(ScreenshotFilePath);
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(SCREENSHOT_WIDTH, SCREENSHOT_HEIGHT, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(element);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (Stream fileStream = File.Create(ScreenshotFilePath))
            {
                pngImage.Save(fileStream);
            }
        }
    }
}
