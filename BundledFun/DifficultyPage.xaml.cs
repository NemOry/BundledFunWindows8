using BundledFun.Classes;
using System;
using System.Collections.Generic;
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

namespace BundledFun
{
    public sealed partial class DifficultyPage : BundledFun.Common.LayoutAwarePage
    {
        public DifficultyPage()
        {
            this.InitializeComponent();
            
            if(Statics.getQuestionsByDifficulty("easy").Count == 0)
            {
                btnEasy.IsEnabled = false;
            }

            if (Statics.getQuestionsByDifficulty("medium").Count == 0)
            {
                btnMedium.IsEnabled = false;
            }

            if (Statics.getQuestionsByDifficulty("hard").Count == 0)
            {
                btnHard.IsEnabled = false;
            }

            if (Statics.questions.Count == 0)
            {
                btnMixed.IsEnabled = false;
            }
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            if (Statics.getQuestionsByDifficulty("easy").Count > 0)
            {
                QuizPage.difficulty = "easy";
                this.Frame.Navigate(typeof(QuizPage));
            }
            else 
            {
                MessageDialog d = new MessageDialog("There are no questions for this category.");
                d.ShowAsync();
            }
        }

        private void btnMedium_Click(object sender, RoutedEventArgs e)
        {
            if (Statics.getQuestionsByDifficulty("medium").Count > 0)
            {
                QuizPage.difficulty = "medium";
                this.Frame.Navigate(typeof(QuizPage));
            }
            else
            {
                MessageDialog d = new MessageDialog("There are no questions for this category.");
                d.ShowAsync();
            }
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            if (Statics.getQuestionsByDifficulty("hard").Count > 0)
            {
                QuizPage.difficulty = "hard";
                this.Frame.Navigate(typeof(QuizPage));
            }
            else
            {
                MessageDialog d = new MessageDialog("There are no questions for this category.");
                d.ShowAsync();
            }
        }

        private void btnMixed_Click(object sender, RoutedEventArgs e)
        {
            if (Statics.questions.Count > 0)
            {
                QuizPage.difficulty = "mixed";
                this.Frame.Navigate(typeof(QuizPage));
            }
            else
            {
                MessageDialog d = new MessageDialog("There are no questions for this category.");
                d.ShowAsync();
            }
        }
    }
}
