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
    public partial class MainPage : PhoneApplicationPage
    {
        int GridRowHeight = 150;
        int ImageHeight = 140;
        int ImageWidth = 140;
        int ImagesPerRow = 3;

        int countList = 0;
        int countListAux = 0;
        List<BitmapImage> images = new List<BitmapImage>();

        public MainPage()
        {
            InitializeComponent();
            PopulateImageGrid();

            var bingContainer = new Bing.BingSearchContainer(
                new Uri("https://api.datamarket.azure.com/Bing/Search/"));

            // replace this value with your account key
            var accountKey = "XXQj7RFCzyb5w0QR0xhLd3g3pFCu4zRuBKhwJ/25Vh0=";

            // the next two lines configure the bingContainer to use your credentials.
            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);

            // note, this line was not required for the C# console app
            bingContainer.UseDefaultCredentials = false;

            // the next two lines define the request for data and 
            var imageQuery = bingContainer.Image("xbox", null, null, null, null, null, null);

            imageQuery.BeginExecute(new AsyncCallback(this.ImageResultLoadedCallback), imageQuery);

        }

        private void ImageResultLoadedCallback(IAsyncResult ar)
        {
            var imageQuery = (DataServiceQuery<Bing.ImageResult>)ar.AsyncState;

            var enumerableImages = imageQuery.EndExecute(ar);

            var imagesList = enumerableImages.ToList();

            // here you could also choose to simply bind the results list to
            // a control in your UI. Instead, we will simply iterate over the
            // results.

            countList = imagesList.Count;
            foreach (var image in imagesList)
            {
                 Dispatcher.BeginInvoke(() =>
                {
                    WebClient wc = new WebClient();
                    wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                    wc.OpenReadAsync(new Uri(image.MediaUrl), wc);
                });
            }
        }


        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                try
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.Result);  

                    Dispatcher.BeginInvoke(() =>
                    {
                        countListAux++;
                        images.Add(image);
                        if (countListAux == countList)
                            PopulateImageGrid2();
                    });
                        
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(ex.Message);
                    });
                   
                }
            }
            else
            {
                //Either cancelled or error handle appropriately for your app
            }
        }

        private void PopulateImageGrid2()
        {
            for (int i = 0; i < images.Count; i += ImagesPerRow)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(GridRowHeight);
                grid1.RowDefinitions.Add(rd);

                int maxPhotosToProcess = (i + ImagesPerRow < images.Count ? i + ImagesPerRow : images.Count);
                int rowNumber = i / ImagesPerRow;
                for (int j = i; j < maxPhotosToProcess; j++)
                {
                    Image img = new Image();
                    img.Height = ImageHeight;
                    img.Stretch = Stretch.Fill;
                    img.Width = ImageWidth;
                    img.HorizontalAlignment = HorizontalAlignment.Center;
                    img.VerticalAlignment = VerticalAlignment.Center;
                    img.Source = images[j];
                    img.SetValue(Grid.RowProperty, rowNumber);
                    img.SetValue(Grid.ColumnProperty, j - i);
                    img.Tap += Image_Tap;
                    grid1.Children.Add(img);
                }
            }
        }

        private void PopulateImageGrid()
        {
            MediaLibrary mediaLibrary = new MediaLibrary();
            var pictures = mediaLibrary.Pictures;

            for (int i = 0; i < pictures.Count; i += ImagesPerRow)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(GridRowHeight);
                grid1.RowDefinitions.Add(rd);

                int maxPhotosToProcess = (i + ImagesPerRow < pictures.Count ? i + ImagesPerRow : pictures.Count);
                int rowNumber = i / ImagesPerRow;
                for (int j = i; j < maxPhotosToProcess; j++)
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(pictures[j].GetImage());

                    Image img = new Image();
                    img.Height = ImageHeight;
                    img.Stretch = Stretch.Fill;
                    img.Width = ImageWidth;
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

        static void MakeRequest()
        {

            
        }

    }

   
}