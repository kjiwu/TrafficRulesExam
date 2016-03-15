using System;
using System.Collections.Generic;
using System.Windows.Input;
using TrafficRulesExam.Helper;
using TrafficRulesExam.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrafficRulesExam.Pages
{
    class MockExamCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (null != _action)
            {
                _action(Convert.ToInt32(parameter));
            }
        }

        Action<int> _action;

        public MockExamCommand(Action<int> action)
        {
            _action = action;
        }
    }

    public class ExamStartPageViewModel : BaseViewModel
    {
        public ExamStartPageViewModel()
        {
            
        }

        public List<int> GetScores()
        {
            UserDataHelper.GetScores();
            return UserDataHelper.Scores;
        }

        public ICommand GotoMockExamCommand
        {
            get
            {
                return new MockExamCommand(MockExam);
            }
        }

        private async void MockExam(int tag)
        {
            Frame root = Window.Current.Content as Frame;
            await UserDataHelper.GetMockExamQuestionIds();
            root.Navigate(typeof(MockExamPage), tag);
        }
    }
}
