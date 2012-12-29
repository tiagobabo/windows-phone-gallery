using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace Gallery
{
    public partial class Home : PhoneApplicationPage
    {
        public Home()
        {
            InitializeComponent();
            ApplicationTitle.Text = Utilities.appName;
            progressBar.Visibility = Visibility.Collapsed;
            PageTitle.Text = "home";
        }

        private void btnLocalStorage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LocalStorage.xaml", UriKind.Relative));
        }

        private void btnOpenFromLink_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
            progressBar.Visibility = Visibility.Visible;
            try
            {
                wc.OpenReadAsync(new Uri(txtURL.Text), wc);
            }
            catch
            {
                 errorMessage();
            }
            
        }

        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                try
                {
                    Image img = new Image();
                    progressBar.Visibility = Visibility.Collapsed;
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.Result);
                    img.Source = image;
                    img.HorizontalAlignment = HorizontalAlignment.Center;
                    img.VerticalAlignment = VerticalAlignment.Center;

                    this.NavigationService.Navigate(new Uri("/ImageViewer.xaml", UriKind.Relative));
                    PhoneApplicationService.Current.State["Image"] = img as Image;
                }
                catch
                {
                    errorMessage();
                }
            }
            else
            {
                errorMessage();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/BingSearch.xaml", UriKind.Relative));
        }

        private void errorMessage()
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("Something went wrong. Try again...");
                progressBar.Visibility = Visibility.Collapsed;
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FlickrSearch.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void txtURL_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtURL.Text == "insert here")
                txtURL.Text = "";
        }
    }
}