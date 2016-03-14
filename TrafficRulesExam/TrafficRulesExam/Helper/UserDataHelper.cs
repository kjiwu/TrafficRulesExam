using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TrafficRulesExam.Models;
using Windows.Storage;

namespace TrafficRulesExam.Helper
{
    public class UserDataHelper
    {
        class UserDataKey
        {
            public static string Subject1ExeOrderKey
            {
                get { return "Subject1ExeOrder"; }
            }

            public static string Subject1ErrorQuestionIdsKey
            {
                get
                {
                    return "Subject1ErrorQuestionIds";
                }
            }

            public static string Subject4ExeOrderKey
            {
                get { return "Subject4ExeOrder"; }
            }

            public static string Subject4ErrorQuestionIdsKey
            {
                get
                {
                    return "Subject1ErrorQuestionIds";
                }
            }

            public static string Subject1MockScoresKey
            {
                get
                {
                    return "Subject1MockScores";
                }
            }

            public static string Subject4MockScoresKey
            {
                get
                {
                    return "Subject4MockScores";
                }
            }
        }

        public static void Save(string key, object value)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = value;
        }

        public static object GetValue(string key)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            return localSettings.Values[key];
        }

        public static void Remove(string key)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(key))
            {
                localSettings.Values.Remove(key);
            }
        }

        public static int SubjectId { get; set; }

        private static Subject Subject1 { get; set; }

        private static Subject Subject4 { get; set; }

        public static Subject GetSubject()
        {
            switch (SubjectId)
            {
                case 1:
                    return Subject1;
                case 4:
                    return Subject4;
            }

            return null;
        }

        public static void SetSubject(int subjectId, Subject subject)
        {
            switch (subjectId)
            {
                case 1:
                    Subject1 = subject;
                    break;
                case 4:
                    Subject4 = subject;
                    break;
            }
        }

        private static List<int> Subject1ErrorQuestionIds
        {
            get;
            set;
        }

        private static List<int> Subject4ErrorQuestionIds
        {
            get;
            set;
        }

        public static int GetSubjectExeOrder()
        {
            object result = null;

            switch (SubjectId)
            {
                case 1:
                    result = GetValue(UserDataKey.Subject1ExeOrderKey);
                    break;
                case 4:
                    result = GetValue(UserDataKey.Subject4ExeOrderKey);
                    break;
            }

            return (null == result) ? 0 : (int)result;
        }

        public static void SaveSubjectExeOrder(int questionId)
        {
            switch (SubjectId)
            {
                case 1:
                    Save(UserDataKey.Subject1ExeOrderKey, questionId);
                    break;
                case 4:
                    Save(UserDataKey.Subject4ExeOrderKey, questionId);
                    break;
            }
        }

        public static void AddErrorQuestionId(int questionId)
        {
            switch (SubjectId)
            {
                case 1:
                    {
                        if (null == Subject1ErrorQuestionIds)
                        {
                            Subject1ErrorQuestionIds = new List<int>();
                        }

                        if (!Subject1ErrorQuestionIds.Contains(questionId))
                        {
                            Subject1ErrorQuestionIds.Add(questionId);
                        }

                        if (Subject1ErrorQuestionIds.Count > 0)
                        {
                            string dataString = GetJsonString(Subject1ErrorQuestionIds);
                            Save(UserDataKey.Subject1ErrorQuestionIdsKey, dataString);
                        }
                    }
                    break;
                case 4:
                    {
                        if (null == Subject4ErrorQuestionIds)
                        {
                            Subject4ErrorQuestionIds = new List<int>();
                        }

                        if (!Subject4ErrorQuestionIds.Contains(questionId))
                        {
                            Subject4ErrorQuestionIds.Add(questionId);
                        }

                        if (Subject4ErrorQuestionIds.Count > 0)
                        {
                            string dataString = GetJsonString(Subject4ErrorQuestionIds);
                            Save(UserDataKey.Subject4ErrorQuestionIdsKey, dataString);
                        }
                    }
                    break;
            }
        }

        private static string GetJsonString(Object value)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(value.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, Subject1ErrorQuestionIds);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        private static T GetJsonObject<T>(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return default(T);
            }

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            var mStream = new MemoryStream(Encoding.UTF8.GetBytes(value));
            return (T)serializer.ReadObject(mStream);
        }

        public static List<int> GetErrorQuestionIds()
        {
            switch (SubjectId)
            {
                case 1:
                    if (null == Subject1ErrorQuestionIds)
                    {
                        var json = (string)GetValue(UserDataKey.Subject1ErrorQuestionIdsKey);
                        if (null != json)
                        {
                            Subject1ErrorQuestionIds = GetJsonObject<List<int>>(json).OrderBy(x => x).ToList();
                        }
                    }
                    return Subject1ErrorQuestionIds;
                case 4:
                    if (null == Subject4ErrorQuestionIds)
                    {
                        var json = (string)GetValue(UserDataKey.Subject4ErrorQuestionIdsKey);
                        if (null != json)
                        {
                            Subject4ErrorQuestionIds = GetJsonObject<List<int>>(json).OrderBy(x => x).ToList();
                        }
                    }
                    return Subject4ErrorQuestionIds;
            }

            return null;
        }

        public static QuestionItem GetQuestion(int questionId)
        {
            Subject subject = GetSubject();
            if (null == subject ||
               null == subject.Exam ||
               null == subject.Exam.Questions)
            {
                return null;
            }

            return subject.Exam.Questions.Where(x => x.Id == questionId).FirstOrDefault();
        }

        #region Get MockExam questions about functions

        public static List<int> MockExamQuestionIds { get; set; }

        public async static Task GetMockExamQuestionIds()
        {
            switch (SubjectId)
            {
                case 1:
                    MockExamQuestionIds = await GetSubject1MockExamQuestionIds();
                    break;
                case 4:
                    MockExamQuestionIds = await GetSubject4MockExamQuestionIds();
                    break;
            }
        }

        private const int Subject1TFQuestionCount = 40;
        private const int Subject1SQuestionCount = 60;

        private async static Task<List<int>> GetSubject1MockExamQuestionIds()
        {
            if (null == Subject1)
            {
                return null;
            }

            return await Task.Run<List<int>>(() =>
            {
                List<int> result = new List<int>();
                var TFQuestions = Subject1.Exam.Questions.Where(x => x.Options.Count == 2).ToList();
                var SQuestions = Subject1.Exam.Questions.Where(x => x.Options.Count > 2).ToList();

                for (int i = 0; i < Subject1TFQuestionCount; i++)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int index = rand.Next(0, TFQuestions.Count() - 1);
                    QuestionItem question = TFQuestions.ElementAt(index);
                    result.Add(question.Id);
                    TFQuestions.Remove(question);
                }

                for (int i = 0; i < Subject1SQuestionCount; i++)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int index = rand.Next(0, SQuestions.Count() - 1);
                    QuestionItem question = SQuestions.ElementAt(index);
                    result.Add(question.Id);
                    SQuestions.Remove(question);
                }

                return result;
            });
        }

        private const int Subject4SQuestionCount = 45;
        private const int Subject4MQuestionCount = 5;

        private async static Task<List<int>> GetSubject4MockExamQuestionIds()
        {
            if (null == Subject4)
            {
                return null;
            }

            return await Task.Run<List<int>>(() =>
            {
                List<int> result = new List<int>();
                var SQuestions = Subject4.Exam.Questions.Where(x => x.Answer.Count == 1).ToList();
                var MQuestions = Subject4.Exam.Questions.Where(x => x.Answer.Count > 1).ToList();

                for (int i = 0; i < Subject4SQuestionCount; i++)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int index = rand.Next(0, SQuestions.Count() - 1);
                    QuestionItem question = SQuestions.ElementAt(index);
                    result.Add(question.Id);
                    SQuestions.Remove(question);
                }

                for (int i = 0; i < Subject4MQuestionCount; i++)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int index = rand.Next(0, MQuestions.Count() - 1);
                    QuestionItem question = MQuestions.ElementAt(index);
                    result.Add(question.Id);
                    MQuestions.Remove(question);
                }

                return result;
            });
        }

        #endregion

        #region Get MockExam score about functions

        public static List<int> Scores
        {
            get; protected set;
        }

        public static void SaveScore(int score)
        {
            if (null == Scores)
            {
                Scores = new List<int>();
            }

            Scores.Add(score);

            if (Scores.Count > 0)
            {
                string dataString = GetJsonString(Scores);
                switch (SubjectId)
                {
                    case 1:
                        Save(UserDataKey.Subject1MockScoresKey, dataString);
                        break;
                    case 4:
                        Save(UserDataKey.Subject4MockScoresKey, dataString);
                        break;
                }

            }

        }

        public static void GetScores()
        {
            string key = UserDataKey.Subject1MockScoresKey;
            switch (SubjectId)
            {
                case 4:
                    key = UserDataKey.Subject4MockScoresKey;
                    break;
            }
            var json = (string)GetValue(key);
            List<int> scores = GetJsonObject<List<int>>(json);
            Scores = scores;
        }

        #endregion

        public static void ClearScores()
        {
            string key = UserDataKey.Subject1MockScoresKey;
            switch (SubjectId)
            {
                case 4:
                    key = UserDataKey.Subject4MockScoresKey;
                    break;
            }
            Remove(key);
            Scores = null;
        }

        public static void ClearExeOrder()
        {
            string key = UserDataKey.Subject1ExeOrderKey;
            switch (SubjectId)
            {
                case 4:
                    key = UserDataKey.Subject4ExeOrderKey;
                    break;
            }
            Remove(key);
        }

        public static void ClearErrorQuestionIds()
        {
            string key = UserDataKey.Subject1ErrorQuestionIdsKey;
            switch (SubjectId)
            {
                case 1:
                    Subject1ErrorQuestionIds = null;
                    break;
                case 4:
                    Subject4ErrorQuestionIds = null;
                    key = UserDataKey.Subject4ErrorQuestionIdsKey;
                    break;
            }
            Remove(key);
        }
    }
}
