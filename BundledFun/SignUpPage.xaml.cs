using BundledFun.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BundledFun
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SignUpPage : BundledFun.Common.LayoutAwarePage
    {
        public SignUpPage()
        {
            this.InitializeComponent();
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            List<Group> groups = await Statics.getAllGroups();
            foreach(Group g in groups)
            {
                listGroups.Items.Add(g.name + ", " + g.id);
            }
           
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {

        }

        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog("");

            progressRing.IsActive = true;
            User user = new User();

            string[] group = listGroups.SelectedItem.ToString().Split(',');

            user.group_id = Convert.ToInt32(group[1].Trim());

            user.name = txtName.Text;
            user.username = txtUsername.Text;
            user.password = txtPassword.Password;
            user.email = txtEmail.Text;
            progressRing.IsActive = false;

            if (user.register() == null)
            {
                dialog.Content = "Username already exists";
            }
            else
            {
                Statics.currentUser = user;
                Statics.currentGroup = await Statics.getGroup(Statics.currentUser.group_id);
                await Statics.bindQuestions(Statics.currentGroup.id);
                dialog.Content = "Registration Successful";
                dialog.ShowAsync();
                this.Frame.Navigate(typeof(MenuPage));
            }
        }
    }
}
