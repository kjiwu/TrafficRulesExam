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
        }

        public async void UpdateUI(int subjectId, QuestionItem question)
        {
            tbkTitle.Text = question.Id.ToString();
            tbkQuestion.Text = question.Question;

            spOptions.Children.Clear();
            foreach (string op in question.Options)
            {
                TextBlock tb = new TextBlock();
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
        }
    }
}
