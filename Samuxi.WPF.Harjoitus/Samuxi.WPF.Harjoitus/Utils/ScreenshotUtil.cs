using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Samuxi.WPF.Harjoitus.Utils
{
    public static class ScreenshotUtil
    {
        public static string ScreenshotFilePath = string.Format(@"{0}\screenshot.bmp",
            System.Windows.Forms.Application.StartupPath);

        public static void TakeScreenshot(UIElement element)
        {
            if (System.IO.File.Exists(ScreenshotFilePath))
            {
                System.IO.File.Delete(ScreenshotFilePath);
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(500, 500, 96, 96, PixelFormats.Pbgra32);
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
