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
    }
}