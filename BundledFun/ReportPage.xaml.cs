using BundledFun.Classes;
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

namespace BundledFun
{
    public sealed partial class ReportPage : BundledFun.Common.LayoutAwarePage
    {
        public static Question question;

        public ReportPage()
        {
            this.InitializeComponent();
            txtQuestion.Text = question.text;
            txtAnswer.Text = question.answer;
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void btnReport_Click(object sender, RoutedEventArgs e)
        {
            var body = "";
            body += "Question: " + question.text + "\n\nAnswer: " + question.answer + "\n\n\nComments: " + txtComments.Text;
            var mailto = new Uri("mailto:?to=nemoryoliver@gmail.com&subject=Question Mistake Report&body=" + body);
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuPage));
        }
    }
}
