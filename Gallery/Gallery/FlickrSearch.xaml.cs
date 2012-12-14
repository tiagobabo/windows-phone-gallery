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
using FlickrNet;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Gallery
{
    public partial class FlickrSearch : PhoneApplicationPage
    {
        Flickr flickr;
        int RowNow = 0;
        int ColumnNow = 0;
        int ResultsPerSearch = 48;

        public FlickrSearch()
        {
            InitializeComponent();
            progressBar.Visibility = Visibility.Visible;
            flickr = new Flickr("922ada222aca8e43571108b76cabbd0e", "10bde29356018553");
            populateGridWithRecentOnes();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text != "")
            {
                grid1.RowDefinitions.Clear();
                grid1.Children.Clear();

                AppSettings settings = new AppSettings();

                progressBar.Visibility = Visibility.Visible;
                PhotoSearchOptions options = new PhotoSearchOptions();
                options.Tags = searchBox.Text;
                options.Page = 1;
                options.PerPage = settings.flickrMaxResults;
                options.Extras = PhotoSearchExtras.LargeSquareUrl;

                flickr.PhotosSearchAsync(options, r =>
                {
                    if (r.Error != null)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("An error occurred talking to Flickr: " + r.Error.Message);
                        });
                        return;
                    }

                    PhotoCollection photos = r.Result;
                    populateGrid(photos);
                });
            }
            else
            {
                progressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show("No results");
            }
        }

        private void populateGridWithRecentOnes()
        {
            PhotoSearchExtras extras = new PhotoSearchExtras();
            flickr.PhotosGetRecentAsync(extras, r =>
            {
                if (r.Error != null)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("An error occurred talking to Flickr: " + r.Error.Message);
                    });
                    return;
                }

                PhotoCollection photos = r.Result;

                populateGrid(photos);

            });
        }

        private void populateGrid(PhotoCollection photos)
        {
            RowNow = 0;
            ColumnNow = 0;

            int results = ResultsPerSearch;
            foreach (var image in photos)
            {
                results--;
                if (results == 0) break;
                WebClient wc = new WebClient();
                wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                wc.OpenReadAsync(new Uri(image.MediumUrl), wc);
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