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
    public class MockExamPageViewModel : BaseViewModel
    {
        public MockExamPageViewModel()
        {
            _questionIds = UserDataHelper.MockExamQuestionIds;
            this.Page = String.Format("{0}/{1}", _currentIndex + 1, _questionIds.Count);
            CurrentQuestion = UserDataHelper.GetQuestion(_questionIds[0]);
        }

        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
        }

        public string _page;

        public string Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                RaisePropertyChanged("Page");
            }
        }
        
        public QuestionItem CurrentQuestion
        {
            get; protected set;
        }

        List<int> _questionIds;

        public event Action<QuestionItem, bool> QuestionChanged;

        int errorCount = 0;

        public void AddErrorCount()
        {
            errorCount++;
        }

        public int GetScore()
        {
            switch (UserDataHelper.SubjectId)
            {
                case 1:
                    return _questionIds.Count - errorCount;
                case 4:
                    return _questionIds.Count * 2 - errorCount * 2;
            }

            return 0;
        }

        public void Next()
        {
            _currentIndex++;
            if (_currentIndex < _questionIds.Count)
            {
                int questionId = _questionIds[_currentIndex];
                CurrentQuestion = UserDataHelper.GetQuestion(questionId);

                if (null != QuestionChanged)
                {
                    QuestionChanged(CurrentQuestion, false);
                }
            }
            else
            {
                if (null != QuestionChanged)
                {
                    QuestionChanged(null, true);
                }
                return;
            }

            this.Page = String.Format("{0}/{1}", _currentIndex + 1, _questionIds.Count);
            UserDataHelper.SaveSubjectExeOrder(_currentIndex);
        }

        public ICommand GetNextCommand
        {
            get
            {
                return new GetQuestionCommand(Next);
            }
        }
    }
}
