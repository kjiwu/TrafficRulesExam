using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrafficRulesExam.CustomContols;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrafficRulesExam.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercisePage : BasePage
    {
        public ExercisePage()
        {
            this.InitializeComponent();
        }               

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            _viewModel = new ExercisePageViewModel();
            _viewModel.LoadQuestionCompleted += _viewModel_LoadQuestionCompleted;
            _viewModel.QuestionChanged += _viewModel_QuestionChanged;
            _viewModel.LoadQuestion();
            UpdateTitle();
        }

        private void _viewModel_QuestionChanged(Models.QuestionItem obj)
        {
            qc.UpdateUI(SubjectId, obj.Id, obj);
        }

        private void _viewModel_LoadQuestionCompleted()
        {
            if (null != _viewModel.Questions)
            {
                var question = _viewModel.Questions[_viewModel.CurrentIndex];
                qc.UpdateUI(SubjectId, question.Id, question);
            }
        }

        private void UpdateTitle()
        {
            switch (SubjectId)
            {
                case 1:
                    tbkTitle.Text = "科目一练习";
                    break;
                case 4:
                    tbkTitle.Text = "科目四练习";
                    break;
            }
        }

        private ExercisePageViewModel _viewModel;
        public ExercisePageViewModel ViewModel
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

        private async void qc_AnwserCompleted(bool obj)
        {
            if (obj)
            {
                _viewModel.Next();
            }
            else
            {
                QuestionItem question = _viewModel.Questions[_viewModel.CurrentIndex];
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
