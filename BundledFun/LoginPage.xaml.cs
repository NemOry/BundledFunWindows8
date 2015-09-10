using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using BundledFun.Classes;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BundledFun
{

    public sealed partial class LoginPage : Page
    {
        private MessageDialog dialog = new MessageDialog("");

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            User user = await Statics.authenticate(txtUsername.Text, txtPassword.Password);

            if (user != null)
            {
                if (user.access != 0)
                {
                    Statics.currentGroup = await Statics.getGroup(user.group_id);
                    if (Statics.currentGroup != null)
                    {
                        await Statics.bindQuestions(Statics.currentGroup.id);
                        Statics.currentUser = user;
                        progressRing.IsActive = false;
                        MessageDialog d = new MessageDialog("Welcome " + user.name, "Authentication Successful");
                        await d.ShowAsync();
                        this.Frame.Navigate(typeof(MenuPage));
                    }
                    else
                    {
                        progressRing.IsActive = false;
                        dialog.Content = "This user exists but does not belong to any group";
                        await dialog.ShowAsync();
                    }
                }
                else 
                {
                    progressRing.IsActive = false;
                    dialog.Content = "Sorry, your account has been disabled. Please contact your group administrator.";
                    await dialog.ShowAsync();
                }
            }
            else 
            {
                progressRing.IsActive = false;
                MessageDialog d = new MessageDialog("Username or Password is incorrect", "Authentication Failed");
                await d.ShowAsync();
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpPage));
        }
    }
}
