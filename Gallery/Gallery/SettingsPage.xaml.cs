using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Gallery
{
    public partial class SettingsPage : PhoneApplicationPage
    {

        AppSettings settings = new AppSettings();
        public SettingsPage()
        {
            InitializeComponent();

            switch (settings.flickrMaxResults)
            {
                case 50:
                    flickrMaxResults.SelectedIndex = 0;
                    break;
                case 30:
                    flickrMaxResults.SelectedIndex = 1;
                    break;
                case 15:
                    flickrMaxResults.SelectedIndex = 2;
                    break;
            }
            switch (settings.bingMaxResults)
            {
                case 50:
                    bingMaxResults.SelectedIndex = 0;
                    break;
                case 30:
                    bingMaxResults.SelectedIndex = 1;
                    break;
                case 15:
                    bingMaxResults.SelectedIndex = 2;
                    break;
            }

        }

        private void flickrMaxResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((string)((ListBoxItem)e.AddedItems[0]).Content);
            settings.flickrMaxResults = Convert.ToInt32(selected);
        }

        private void bingMaxResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((string)((ListBoxItem)e.AddedItems[0]).Content);
            settings.bingMaxResults = Convert.ToInt32(selected);
        }

       
    }
}