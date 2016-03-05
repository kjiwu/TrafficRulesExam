using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
using TrafficRulesExam.ViewModels;

namespace TrafficRulesExam.Pages
{
    public class GetQuestionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        Action _param;

        public void Execute(object parameter)
        {
            if(null != _param)
            {
                _param();
            }
        }

        public GetQuestionCommand(Action param)
        {
            _param = param;
        }
    }

    public class ExercisePageViewModel : BaseViewModel
    {
        public ExercisePageViewModel(int subjectId)
        {
            _subjectId = subjectId;
            LoadQuestion();
        }

        private int _subjectId;

        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = value;
            }
        }

        private QuestionItem _currentQuestion;

        private List<QuestionItem> _questions;
        public List<QuestionItem> Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
            }
        }

        public event Action LoadQuestionCompleted;
        public event Action<QuestionItem> QuestionChanged;

        private void LoadQuestion()
        {
            HttpHelper.GetExam(_subjectId, subject =>
            {
                Questions = subject.Exam.Questions;

                if(null != LoadQuestionCompleted)
                {
                    LoadQuestionCompleted();
                }
            });
        }

        public void Next()
        {
            _currentIndex++;
            if (_currentIndex < Questions.Count)
            {
                _currentQuestion = Questions[_currentIndex];
                if(null != QuestionChanged)
                {
                    QuestionChanged(_currentQuestion);
                }
            }
        }

        public void Perior()
        {
            _currentIndex--;
            if (_currentIndex >= 0)
            {
                _currentQuestion = Questions[_currentIndex];
                if (null != QuestionChanged)
                {
                    QuestionChanged(_currentQuestion);
                }
            }
        }

        public ICommand GetNextCommand
        {
            get
            {
                return new GetQuestionCommand(Next);              
            }
        }

        public ICommand GetPeriorCommand
        {
            get
            {
                return new GetQuestionCommand(Perior);
            }
        }
    }
}
