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
using Facebook;
using Microsoft.Phone.Shell;

namespace Gallery
{
    public partial class FacebookInfoPage : PhoneApplicationPage
    {
        private string _accessToken;
        private string _userId;
        int RowNow = 0;
        int ColumnNow = 0;
        int ResultsPerSearch = 48;

        public FacebookInfoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _accessToken = NavigationContext.QueryString["access_token"];
            _userId = NavigationContext.QueryString["id"];
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFacebookData();
        }

        private void LoadFacebookData()
        {
            GetPhotos();
        }

       
        private void GetPhotos()
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var data = (IList<object>)result["data"];

                RowNow = 0;
                ColumnNow = 0;

                int results = ResultsPerSearch;
                for (int i = 0; i < data.Count; i++)
                {
                    result = (IDictionary<string, object>)data[i];
                    var photos = (IList<object>)result["images"];

                    if (photos != null)
                    {
                        var photo = (IDictionary<string, object>)photos[3];
                        results--;
                        if (results == 0) break;

                        WebClient wc = new WebClient();
                        wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                        wc.OpenReadAsync(new Uri((string)photo["source"]), wc);
                    }
                }
            };

            fb.GetAsync("me/photos");
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
                        catch
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }

    }
}