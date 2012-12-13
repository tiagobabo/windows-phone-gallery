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
using Microsoft.Phone.Shell;
using System.Data.Services.Client;

namespace Gallery
{
    public partial class LocalStorage : PhoneApplicationPage
    {

        public LocalStorage()
        {
            InitializeComponent();
            ApplicationTitle.Text = Utilities.appName;
            PageTitle.Text = "local storage";
            PopulateImageGrid();
        }

        private void PopulateImageGrid()
        {
            MediaLibrary mediaLibrary = new MediaLibrary();
            var pictures = mediaLibrary.Pictures;

            for (int i = 0; i < pictures.Count; i += Utilities.ImagesPerRow)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(Utilities.GridRowHeight);
                grid1.RowDefinitions.Add(rd);

                int maxPhotosToProcess = (i + Utilities.ImagesPerRow < pictures.Count ? i + Utilities.ImagesPerRow : pictures.Count);
                int rowNumber = i / Utilities.ImagesPerRow;
                for (int j = i; j < maxPhotosToProcess; j++)
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(pictures[j].GetImage());

                    Image img = new Image();
                    img.Height = Utilities.ImageHeight;
                    img.Stretch = Stretch.Fill;
                    img.Width = Utilities.ImageWidth;
                    img.HorizontalAlignment = HorizontalAlignment.Center;
                    img.VerticalAlignment = VerticalAlignment.Center;
                    img.Source = image;
                    img.SetValue(Grid.RowProperty, rowNumber);
                    img.SetValue(Grid.ColumnProperty, j - i);
                    img.Tap += Image_Tap;
                    grid1.Children.Add(img);
                }
            }
        }

        private void Image_Tap(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ImageViewer.xaml", UriKind.Relative));
            PhoneApplicationService.Current.State["Image"] = sender as Image;
        }

    }

}