﻿using System;
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
            wc.OpenReadAsync(new Uri("http://www.cardioaccess.com/OpImages/asd1.jpg"), wc);
        }

        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                try
                {
                    Image img = new Image();
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.Result);
                    img.Source = image;
                    img.HorizontalAlignment = HorizontalAlignment.Center;
                    img.VerticalAlignment = VerticalAlignment.Center;

                    this.NavigationService.Navigate(new Uri("/ImageViewer.xaml", UriKind.Relative));
                    PhoneApplicationService.Current.State["Image"] = img as Image;
                }
                catch (Exception ex)
                {
                    //Exception handle appropriately for your app
                }
            }
            else
            {
                //Either cancelled or error handle appropriately for your app
            }
        }
    }
}