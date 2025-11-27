using GreenerConfigurator.ClientCore.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GreenerConfigurator.Utilities
{
    public static class ImageHelper
    {
        public static void AddListOfDataOnImage(Bitmap bitMap, List<ImageExtraData> ExtraDataList)
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                using (Font arialFont = new Font("Arial", 10))
                {
                    foreach (var item in ExtraDataList)
                    {
                        PointF location = new PointF(item.PositionX, item.PositionY);
                        graphics.DrawString(item.Label, arialFont, Brushes.White, location);
                    }
                }
            }
        }

        public static void AddDataOnImage(Bitmap bitMap, string text, float x, float y)
        {
            PointF location = new PointF(x, y);
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                using (Font arialFont = new Font("Arial", 10))
                {
                    graphics.DrawString(text, arialFont, Brushes.White, location);
                }
            }
        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        public static Bitmap BitmapFromBase64(string base64)
        {
            byte[] imageAsBytes = Convert.FromBase64String(base64);
            MemoryStream memoryStream = new MemoryStream(imageAsBytes);
            memoryStream.Position = 0;
            var bitMap = (Bitmap)Bitmap.FromStream(memoryStream);

            return bitMap;
        }

        public static BitmapSource BitmapSourceFromBase64(string base64)
        {
            byte[] imageAsBytes = Convert.FromBase64String(base64);
            MemoryStream memoryStream = new MemoryStream(imageAsBytes);
            memoryStream.Position = 0;
            var bitMap = (Bitmap)Bitmap.FromStream(memoryStream);

            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                                       bitMap.GetHbitmap(),
                                                       IntPtr.Zero,
                                                       System.Windows.Int32Rect.Empty,
                                                       BitmapSizeOptions.FromWidthAndHeight(bitMap.Width, bitMap.Height));

            return bitmapSource;
        }

        public static BitmapSource BitmapSourceBitMap(Bitmap bitmap)
        {
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                                       bitmap.GetHbitmap(),
                                                       IntPtr.Zero,
                                                       System.Windows.Int32Rect.Empty,
                                                       BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

            return bitmapSource;
        }

        public static string GetImageMimeType(string imageFileAddress)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(imageFileAddress))
            {
                var tempExtenstion = Path.GetExtension(imageFileAddress);
                
                imageMimeTypeMappings.TryGetValue(tempExtenstion, out result);
            }

            return result;
        }

        public class ImageExtraData
        {
            public string Label { get; set; }

            public float PositionX { get; set; }

            public float PositionY { get; set; }
        }


        private static IDictionary<string, string> imageMimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {".gif", "image/gif"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
        };

    }
}
