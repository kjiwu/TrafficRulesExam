using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TrafficRulesExam.UserControls
{
    public sealed partial class QuestionUserControl : UserControl
    {
        public QuestionUserControl()
        {
            this.InitializeComponent();
            IsMultiSelect = false;
        }

        public bool IsMultiSelect {
            set
            {
                btnMultiSelect.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        QuestionItem _question;

        public async void UpdateUI(int subjectId, QuestionItem question)
        {
            _question = question;
            answers = new List<int>();

            tbkTitle.Text = question.Id.ToString();
            tbkQuestion.Text = question.Question;

            spOptions.Children.Clear();
            foreach (string op in question.Options)
            {
                TextBlock tb = new TextBlock();
                tb.Margin = new Thickness(0, 4, 0, 4);
                tb.Text = op;
                tb.TextWrapping = TextWrapping.Wrap;
                spOptions.Children.Add(tb);
            }

            if (question.Image == 1)
            {
                imageControl.Visibility = Visibility.Visible;
                IRandomAccessStream stream = await QuestionHelper.GetQuestionImage(subjectId, question.Id);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);

                imageControl.Source = image;
                imageControl.Stretch = Stretch.UniformToFill;
                imageControl.Height = image.PixelHeight;
                imageControl.Width = image.PixelWidth;
            }
            else
            {
                imageControl.Visibility = Visibility.Collapsed;
            }

            GenerateButtons();
        }

        private void GenerateButtons()
        {
            if (null == _question)
                return;

            answerContainer.Children.Clear();
            switch (_question.Options.Count)
            {
                case 2:
                    {
                        ToggleButton rightButton = new ToggleButton();
                        rightButton.Content = _question.Options[0];
                        rightButton.SetValue(Grid.ColumnProperty, 0);
                        rightButton.Margin = new Thickness(4, 0, 4, 0);
                        rightButton.Tag = 1;
                        rightButton.Tapped += ToggleButton_Tapped;

                        ToggleButton wrongButton = new ToggleButton();
                        wrongButton.Content = _question.Options[1];
                        wrongButton.SetValue(Grid.ColumnProperty, 1);
                        wrongButton.Margin = new Thickness(4, 0, 4, 0);
                        wrongButton.Tag = 2;
                        wrongButton.Tapped += ToggleButton_Tapped;

                        answerContainer.Children.Add(rightButton);
                        answerContainer.Children.Add(wrongButton);
                    }
                    break;
                case 4:
                    {
                        ToggleButton AButton = new ToggleButton();
                        AButton.Content = "A";
                        AButton.SetValue(Grid.ColumnProperty, 0);
                        AButton.Margin = new Thickness(4, 0, 4, 0);
                        AButton.Tag = 1;
                        AButton.Tapped += ToggleButton_Tapped;

                        ToggleButton BButton = new ToggleButton();
                        BButton.Content = "B";
                        BButton.SetValue(Grid.ColumnProperty, 1);
                        BButton.Margin = new Thickness(4, 0, 4, 0);
                        BButton.Tag = 2;
                        BButton.Tapped += ToggleButton_Tapped;

                        ToggleButton CButton = new ToggleButton();
                        CButton.Content = "C";
                        CButton.SetValue(Grid.ColumnProperty, 2);
                        CButton.Margin = new Thickness(4, 0, 4, 0);
                        CButton.Tag = 3;
                        CButton.Tapped += ToggleButton_Tapped;

                        ToggleButton DButton = new ToggleButton();
                        DButton.Content = "D";
                        DButton.SetValue(Grid.ColumnProperty, 3);
                        DButton.Margin = new Thickness(4, 0, 4, 0);
                        DButton.Tag = 4;
                        DButton.Tapped += ToggleButton_Tapped;

                        answerContainer.Children.Add(AButton);
                        answerContainer.Children.Add(BButton);
                        answerContainer.Children.Add(CButton);
                        answerContainer.Children.Add(DButton);
                    }
                    break;
            }
        }

        List<int> answers = new List<int>();

        public event Action<bool> AnwserCompleted;

        private void ToggleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            int answer = (int)button.Tag;
            if((bool)button.IsChecked)
            {
                answers.Add(answer);
                bool result = GetAnwserResult();
                if (null != AnwserCompleted)
                {
                    AnwserCompleted(result);
                }
            }
            else
            {
                if (answers.Contains(answer))
                {
                    answers.Remove(answer);
                }
            }
        }

        private bool GetAnwserResult()
        {
            if(answers.Count != _question.Answer.Count)
            {
                return false;
            }
            else
            {
                bool result = true;
                foreach(int answer in answers)
                {
                    if (!_question.Answer.Contains(answer))
                    {
                        result = false;
                        break;
                    }
                }
                return result;
            }
        }

        private void btnMultiSelect_Click(object sender, RoutedEventArgs e)
        {
            bool result = GetAnwserResult();
            if(null != AnwserCompleted)
            {
                AnwserCompleted(result);
            }
        }
    }
}
