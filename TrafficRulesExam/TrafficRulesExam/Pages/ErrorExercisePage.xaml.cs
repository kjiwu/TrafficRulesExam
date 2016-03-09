using TrafficRulesExam.CustomContols;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrafficRulesExam.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ErrorExercisePage : BasePage
    {
        public ErrorExercisePage()
        {
            this.InitializeComponent();
        }

        ErrorExercisePageViewModel _viewModel;
        public ErrorExercisePageViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _viewModel = new ErrorExercisePageViewModel();
            _viewModel.QuestionChanged += _viewModel_QuestionChanged;
            _viewModel.LoadQuestion();
            qc.UpdateUI(SubjectId, _viewModel.CurrentQuestion);
        }

        private void _viewModel_QuestionChanged(Models.QuestionItem obj)
        {
            qc.UpdateUI(SubjectId, obj);
        }

        private async void qc_AnwserCompleted(bool obj)
        {
            if (obj)
            {
                _viewModel.Next();
            }
            else
            {
                QuestionItem question = _viewModel.CurrentQuestion;
                ContentDialog dialog = new ContentDialog()
                {
                    Content = question.Explain,
                    Title = "解释",
                    Background = new SolidColorBrush(Colors.Cornsilk),
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();

                UserDataHelper.AddErrorQuestionId(question.Id);
            }
        }
    }
}
