using BundledFun.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BundledFun
{
    public sealed partial class MenuPage : Page
    {
        public MenuPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private async void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (Statics.questions.Count > 0)
            {
                this.Frame.Navigate(typeof(DifficultyPage));
            }
            else
            {
                MessageDialog d = new MessageDialog("There are no questions to answer. Please contact your group administrator.");
                d.ShowAsync();
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Statics.questions.Clear();
            Statics.currentGroup = null;
            Statics.currentUser = null;
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
