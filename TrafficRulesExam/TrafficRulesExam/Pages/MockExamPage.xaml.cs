using TrafficRulesExam.CustomContols;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrafficRulesExam.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MockExamPage : BasePage
    {
        public MockExamPage()
        {
            this.InitializeComponent();
        }

        MockExamPageViewModel _viewModel;
        public MockExamPageViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = new MockExamPageViewModel();
            _viewModel.QuestionChanged += _viewModel_QuestionChanged;
            UpdateUI();   
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _viewModel.QuestionChanged -= _viewModel_QuestionChanged;
            _viewModel = null;
        }

        private async void _viewModel_QuestionChanged(QuestionItem question, bool isCompleted)
        {
            if (!isCompleted)
            {
                qc.UpdateUI(UserDataHelper.SubjectId, _viewModel.CurrentIndex + 1, question);
            }
            else
            {
                int score = _viewModel.GetScore();
                string message = score >= 90 ? String.Format("{0}分, 恭喜过关", score) :
                    String.Format("{0}分, 遗憾未通过，继续努力！", score);

                ContentDialog dialog = new ContentDialog()
                {
                    Content = message,
                    Title = "最终成绩",
                    Background = new SolidColorBrush(Colors.Cornsilk),
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();

                UserDataHelper.SaveScore(score);

                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private void UpdateUI()
        {
            var questions = UserDataHelper.MockExamQuestionIds;
            if(null != questions)
            {
                QuestionItem question = UserDataHelper.GetQuestion(questions.First());
                qc.UpdateUI(UserDataHelper.SubjectId, _viewModel.CurrentIndex + 1, question);
            }
        }

        private async void qc_AnwserCompleted(bool obj)
        {
            if (!obj)
            {
                QuestionItem question = _viewModel.CurrentQuestion;
                ContentDialog dialog = new ContentDialog()
                {
                    Content = QuestionItem.GetExplain(question),
                    Title = "正确答案：",
                    Background = new SolidColorBrush(Colors.Cornsilk),
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
                _viewModel.AddErrorCount();
                UserDataHelper.AddErrorQuestionId(question.Id);
                _viewModel.Next();
            }
            else
            {
                _viewModel.Next();
            }
        }
    }
}
