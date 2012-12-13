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
using System.Data.Services.Client;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;

namespace Gallery
{
    public partial class BingSearch : PhoneApplicationPage
    {
        int RowNow = 0;
        int ColumnNow = 0;

        public BingSearch()
        {
            InitializeComponent();
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text != "")
            {
                progressBar.Visibility = Visibility.Visible;
                grid1.RowDefinitions.Clear();
                grid1.Children.Clear();

                var accountKey = "XXQj7RFCzyb5w0QR0xhLd3g3pFCu4zRuBKhwJ/25Vh0=";
                
                var bingContainer = new Bing.BingSearchContainer(
                    new Uri("https://api.datamarket.azure.com/Bing/Search/"));
                bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);
                bingContainer.UseDefaultCredentials = false;

                var imageQuery = bingContainer.Image(searchBox.Text, null, "en-US", null, null, null, "Size:Medium");
                imageQuery.BeginExecute(new AsyncCallback(this.ImageResultLoadedCallback), imageQuery);
            }
            else
            {
                progressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show("No results");
            }
        }

        private void ImageResultLoadedCallback(IAsyncResult ar)
        {
            var imageQuery = (DataServiceQuery<Bing.ImageResult>)ar.AsyncState;
            var enumerableImages = imageQuery.EndExecute(ar);
            var imagesList = enumerableImages.ToList();

            if (imagesList.Count == 0)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("No results");
                });
                
                return;
            }

            RowNow = 0;
            ColumnNow = 0;
            foreach (var image in imagesList)
            {
                WebClient wc = new WebClient();
                wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                wc.OpenReadAsync(new Uri(image.MediaUrl), wc);
            }
        }


        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                try
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        BitmapImage image = new BitmapImage();
                        try
                        {
                            image.SetSource(e.Result);
                        }
                        catch (Exception) 
                        {
                            return;
                        }

                        PopulateImageGrid2(image);
                    });

                }
                catch
                {
                    return;
                }
            }
        }

        private void errorMessage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Something went wrong. Try again...");
            });
        }

        private void PopulateImageGrid2(BitmapImage image)
        {
            progressBar.Visibility = Visibility.Collapsed;
            if (ColumnNow == 0)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(Utilities.GridRowHeight);
                grid1.RowDefinitions.Add(rd);
            }

            Image img = new Image();
            img.Height = Utilities.ImageHeight;
            img.Stretch = Stretch.Fill;
            img.Width = Utilities.ImageWidth;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Source = image;
            img.SetValue(Grid.RowProperty, RowNow);
            img.SetValue(Grid.ColumnProperty, ColumnNow);
            img.Tap += Image_Tap;
            grid1.Children.Add(img);

            ColumnNow++;

            if (ColumnNow == Utilities.ImagesPerRow)
            {
                ColumnNow = 0;
                RowNow++;
            }
        }

        private void Image_Tap(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/ImageViewer.xaml", UriKind.Relative));
            PhoneApplicationService.Current.State["Image"] = sender as Image;
        }
    }
}