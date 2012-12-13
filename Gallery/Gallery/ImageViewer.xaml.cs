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
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace Gallery
{
    public partial class ImageViewer : PhoneApplicationPage
    {
        double startingPositionOfImageX;
        double startingPositionOfImageY;
        Image image;

        public ImageViewer()
        { 
            InitializeComponent();
            image = PhoneApplicationService.Current.State["Image"] as Image;
            image1.Source = image.Source;
            image1.ManipulationStarted += Image_ManipulationStarted;
            image1.ManipulationDelta += Image_ManipulationDelta;
        }

        private void Zoom_add_Click(object sender, RoutedEventArgs e)
        {
            transform.ScaleX *= 1.1;
            transform.ScaleY *= 1.1;
        }

        private void Zoom_remove_Click(object sender, RoutedEventArgs e)
        {
            transform.ScaleX *= 0.9;
            transform.ScaleY *= 0.9;
        }

        private void Rot_add_Click(object sender, RoutedEventArgs e)
        {
            transform.Rotation += 10.0;
        }

        private void Rot_remove_Click(object sender, RoutedEventArgs e)
        {
            transform.Rotation -= 10.0;
        }

        private void Save_image_Click(object sender, RoutedEventArgs e)
        {
            Utilities.SaveImage((BitmapImage) image.Source, "image.jpg", 0, 85);
            MessageBox.Show("Image saved.");
        }

        private void Image_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            startingPositionOfImageX = transform.TranslateX;
            startingPositionOfImageY = transform.TranslateY;
        }

        private void Image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            double previousRotation = transform.Rotation;
            transform.Rotation = 0.0;

            transform.TranslateX = e.CumulativeManipulation.Translation.X * transform.ScaleX + startingPositionOfImageX;
            transform.TranslateY = e.CumulativeManipulation.Translation.Y * transform.ScaleY + startingPositionOfImageY;

            transform.Rotation = previousRotation;
        }



    }
}