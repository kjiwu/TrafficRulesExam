using System;
using System.Diagnostics;
using System.Windows.Input;
using TrafficRulesExam.Pages;
using TrafficRulesExam.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrafficRulesExam
{
    public class GotoSubjectCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(null != _param)
            {
                int subjectId = Convert.ToInt32(parameter.ToString());
                _param(subjectId);
            }
        }

        private Action<int> _param;

        public GotoSubjectCommand(Action<int> param)
        {
            _param = param;
        }
    }

    public class MainPageViewModel : BaseViewModel
    {
        public ICommand GotoSubjectCommand
        {
            get
            {
                return new GotoSubjectCommand(GotoSubject);
            }
        }

        private void GotoSubject(int subjectId)
        {
            Debug.WriteLine(subjectId);
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(SubjectStartPage), subjectId);
        }
    }
}
