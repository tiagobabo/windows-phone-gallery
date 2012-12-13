using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;

namespace Gallery
{
    class Utilities
    {
        public static string appName = "GALLERY";
        public static int GridRowHeight = 150;
        public static int ImageHeight = 140;
        public static int ImageWidth = 140;
        public static int ImagesPerRow = 3;

        public static void SaveImage(BitmapImage bitmap, string fileName, int orientation, int quality)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(fileName))
                    isolatedStorage.DeleteFile(fileName);
                IsolatedStorageFileStream fileStream = isolatedStorage.CreateFile(fileName);
                
                WriteableBitmap wb = new WriteableBitmap(bitmap);
                wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, orientation, quality);

                fileStream.Close();

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (fileStream = myIsolatedStorage.OpenFile("image.jpg", FileMode.Open, FileAccess.Read))
                    {
                        MediaLibrary mediaLibrary = new MediaLibrary();
                        Picture pic = mediaLibrary.SavePicture("image.jpg", fileStream);
                        fileStream.Close();
                    }
                }
            }
        }
    }
}
