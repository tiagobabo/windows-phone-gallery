using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;

namespace Gallery
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            MediaLibrary mediaLibrary = new MediaLibrary();
            var pictures = mediaLibrary.Pictures;
            foreach (var picture in pictures)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(picture.GetImage());

                MediaImage mediaImage = new MediaImage();
                mediaImage.ImageFile = image;

                listBox1.Items.Add(mediaImage);
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MediaLibrary mediaLibrary = new MediaLibrary();
            BitmapImage image = new BitmapImage();
            image.SetSource(mediaLibrary.Pictures[listBox1.SelectedIndex].GetImage());
            image1.Source = image;
        }
    }

   
}