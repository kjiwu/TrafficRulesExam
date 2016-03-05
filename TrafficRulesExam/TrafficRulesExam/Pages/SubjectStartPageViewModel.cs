using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrafficRulesExam.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrafficRulesExam.Pages
{
    public class GotoMockExamCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        Action<int> _param;

        public void Execute(object parameter)
        {
            if(null != _param)
            {
                _param(Convert.ToInt32(parameter));
            }
        }

        public GotoMockExamCommand(Action<int> param)
        {
            _param = param;
        }
    }

    public class SubjectStartPageViewModel : BaseViewModel
    {
        public ICommand GotoMockExamCommand
        {
            get
            {
                return new GotoMockExamCommand(GotoMockExam);
            }
        }

        private void GotoMockExam(int subjectId)
        {
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(ExamStartPage), subjectId);
        }
    }
}
