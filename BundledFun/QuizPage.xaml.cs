using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
using Newtonsoft.Json;
using BundledFun.Classes;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.Storage;
using Windows.ApplicationModel;

namespace BundledFun
{
    public sealed partial class QuizPage : Page
    {
        private Question currentQuestion;
        private int totalScore = 0;
        private MessageDialog dialog = new MessageDialog("");
        private MediaElement correctSound = new MediaElement();
        private MediaElement wrongSound = new MediaElement();
        private int timerCounter = 1;
        private DispatcherTimer timer;
        private int totalTimeElapsed = 0;
        private int totalCorrectAnswers = 0;

        public static string difficulty = "";

        private List<Question> current_questions = new List<Question>();
        private List<Question> skipped_questions = new List<Question>();

        private bool isSkipMode = false;

        public QuizPage()
        {
            this.InitializeComponent();
            this.initialize();
            this.confirmStart();
        }

        private void timer_Tick(object sender, object e)
        {
            int timeLeft = currentQuestion.timer - timerCounter;
            if (timeLeft >= 0)
            {
                lblTimer.Text = timeLeft + "";
            }
            else
            {
                timerCounter = 0;
                this.nextQuestion();
            }

            totalTimeElapsed++;

            timerCounter++;
        }

        private async void initialize()
        {
            current_questions = Statics.getQuestionsByDifficulty(difficulty);

            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync("Audios");
            StorageFile file = await folder.GetFileAsync("correct.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            correctSound.SetSource(stream, file.ContentType);

            file = await folder.GetFileAsync("wrong.wav");
            stream = await file.OpenAsync(FileAccessMode.Read);
            wrongSound.SetSource(stream, file.ContentType);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += timer_Tick;
        }

        private async void confirmStart()
        {
            MessageDialog confirm = new MessageDialog("", "Choose what to do");
            confirm.Commands.Add(new UICommand("Back to main menu", null, 0));
            confirm.Commands.Add(new UICommand("Start", null, 1));
            confirm.DefaultCommandIndex = 1;
            IUICommand response = await confirm.ShowAsync();

            if ((int)response.Id == 1)
            {
                this.start();
            }
            else
            {
                this.Frame.Navigate(typeof(MenuPage));
            }
        }

        private void start()
        {
            if(current_questions.Count <= 2)
            {
                btnSkip.IsEnabled = false;
            }

            Statics.shuffleList(current_questions);
            currentQuestion = current_questions.ElementAt<Question>(0);
            this.displayQuestion(currentQuestion);
        }

        private void restart()
        {
            btnSkip.IsEnabled = true;
            lblSkippedQuestions.Text = "";
            this.isSkipMode = false;
            this.skipped_questions.Clear();
            this.totalCorrectAnswers = 0;
            this.totalTimeElapsed = 0;
            this.current_questions.Clear();
            this.current_questions = Statics.getQuestionsByDifficulty(difficulty);
            this.totalScore = 0;
            this.start();
        }

        private void pause()
        {
            timer.Stop();

            btnA.IsEnabled = false;
            btnB.IsEnabled = false;
            btnC.IsEnabled = false;
            btnSkip.IsEnabled = false;

            if (media.CurrentState == MediaElementState.Playing)
            {
                media.Pause();
            }
        }

        private void resume()
        {
            timer.Start();

            btnA.IsEnabled = true;
            btnB.IsEnabled = true;
            btnC.IsEnabled = true;
            btnSkip.IsEnabled = true;

            if (media.CurrentState == MediaElementState.Paused)
            {
                media.Play();
            }
        }

        private async void save()
        {
            if ( await Statics.currentUser.saveScore(totalScore, totalTimeElapsed, totalCorrectAnswers) == true)
            {
                dialog.Content = "Score Saved";
            }
            else
            {
                dialog.Content = "Oops! We did something wrong.";
            }

            dialog.ShowAsync();
            this.Frame.Navigate(typeof(MenuPage));
        }

        private void displayQuestion(Question q)
        {
            btnA.Content = q.choice_a;
            btnB.Content = q.choice_b;
            btnC.Content = q.choice_c;

            media.Source = null;
            imgQuestion.Source = null;

            if(q.type.Equals("image"))
            {
                txtQuestion.Text = q.text;
                txtQuestion.Visibility = Visibility.Visible;
                media.Visibility = Visibility.Collapsed;
                imgQuestion.Visibility = Visibility.Visible;
                txtQuestionBig.Visibility = Visibility.Collapsed;
                imgQuestion.Source = new BitmapImage(new Uri(Statics.hostName + "/public/groups/" + Statics.currentGroup.name + "/files/questions/" + q.file, UriKind.Absolute));
            }
            else if (q.type.Equals("video"))
            {
                txtQuestion.Text = q.text;
                txtQuestion.Visibility = Visibility.Visible;
                imgQuestion.Visibility = Visibility.Collapsed;
                txtQuestionBig.Visibility = Visibility.Collapsed;
                media.Visibility = Visibility.Visible;
                media.Source = new Uri(Statics.hostName + "/public/groups/" + Statics.currentGroup.name + "/files/questions/" + q.file, UriKind.Absolute);
                media.Play();
            }
            else if (q.type.Equals("audio"))
            {
                txtQuestion.Text = q.text;
                txtQuestion.Visibility = Visibility.Visible;
                imgQuestion.Visibility = Visibility.Collapsed;
                txtQuestionBig.Visibility = Visibility.Collapsed;
                media.Visibility = Visibility.Visible;
                media.Source = new Uri(Statics.hostName + "/public/groups/" + Statics.currentGroup.name + "/files/questions/" + q.file, UriKind.Absolute);
                media.Play();
            }
            else if (q.type.Equals("text"))
            {
                txtQuestionBig.Text = q.text;
                media.Visibility = Visibility.Visible;
                imgQuestion.Visibility = Visibility.Collapsed;
                txtQuestion.Visibility = Visibility.Collapsed;
                txtQuestionBig.Visibility = Visibility.Visible;
            }

            timer.Stop();
            timerCounter = 0;
            timer.Start();
        }

        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            this.compare((string) btnA.Content);
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            this.compare((string)btnB.Content);
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            this.compare((string)btnC.Content);
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            skipped_questions.Add(currentQuestion);
            current_questions.RemoveAt(0);

            if(current_questions.Count <= 2)
            {
                btnSkip.IsEnabled = false;
            }

            this.nextQuestion();
        }

        private void nextQuestion()
        {
            if (isSkipMode)
            {
                if (skipped_questions.Count > 0)
                {
                    Statics.shuffleList(skipped_questions);
                    currentQuestion = skipped_questions.ElementAt<Question>(0);
                }
                else 
                {
                    finish();
                    return;
                }
            }
            else
            {
                if (current_questions.Count > 0)
                {
                    Statics.shuffleList(current_questions);
                    currentQuestion = current_questions.ElementAt<Question>(0);
                }
            }

            Debug.WriteLine(currentQuestion.text);

            this.displayQuestion(currentQuestion);
        }

        private async void compare(string answer)
        {
            timer.Stop();
            media.Stop();

            if (answer.Trim().ToUpper().Equals(currentQuestion.answer.Trim().ToUpper()))
            {
                totalCorrectAnswers++;
                this.totalScore++;
                correctSound.Play();
            }
            else 
            {
                wrongSound.Play();
                dialog.Content = "Sorry, the correct answer is: " + currentQuestion.answer;
                await dialog.ShowAsync();
            }

            if (isSkipMode)
            {
                if (skipped_questions.Count > 0)
                {
                    skipped_questions.RemoveAt(0);
                }
                else
                {
                    finish();
                    return;
                }
            }
            else 
            {
                if (current_questions.Count > 0)
                {
                    current_questions.RemoveAt(0);
                }
            }

            if(current_questions.Count <= 2)
            {
                btnSkip.IsEnabled = false;
            }

            if (current_questions.Count == 0 && skipped_questions.Count > 0)
            {
                isSkipMode = true;
            }
            else if (current_questions.Count == 0 && skipped_questions.Count == 0)
            {
                this.finish();
                return;
            }

            lblSkippedQuestions.Text = "Skipped Questions: " + skipped_questions.Count;

            if(current_questions.Count == 0 && skipped_questions.Count == 0)
            {
                finish();
                return;
            }

            this.nextQuestion();
        }

        private async void finish()
        {
            MessageDialog d = new MessageDialog("", "Finished");
            d.Content = "Finished with a total score of " + totalScore;
            d.Commands.Add(new UICommand("Back to main menu", null, 0));
            d.Commands.Add(new UICommand("Restart", null, 1));
            d.Commands.Add(new UICommand("Save", null, 2));
            d.DefaultCommandIndex = 1;
            IUICommand response = await d.ShowAsync();

            if ((int)response.Id == 0)
            {
                this.Frame.Navigate(typeof(MenuPage));
            }
            else if ((int)response.Id == 1)
            {
                this.restart();
            }
            else if ((int)response.Id == 2)
            {
                this.save();
            }
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if(btnPlayPause.Content.Equals("Pause"))
            {
                this.pause();
                btnPlayPause.Content = "Play";
            }
            else if (btnPlayPause.Content.Equals("Play"))
            {
                this.resume();
                btnPlayPause.Content = "Pause";
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuPage));
        }

        private async void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ReportPage.question = currentQuestion;
            this.Frame.Navigate(typeof(ReportPage));
        }
    }
}
