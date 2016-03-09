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
            switch(subjectId)
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
                        if(null == Subject1ErrorQuestionIds)
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
            if(String.IsNullOrEmpty(value))
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
                        Subject1ErrorQuestionIds = GetJsonObject<List<int>>(json).OrderBy(x => x).ToList();
                    }
                    return Subject1ErrorQuestionIds;
                case 4:
                    if (null == Subject4ErrorQuestionIds)
                    {
                        var json = (string)GetValue(UserDataKey.Subject4ErrorQuestionIdsKey);
                        Subject4ErrorQuestionIds = GetJsonObject<List<int>>(json).OrderBy(x => x).ToList();
                    }
                    return Subject4ErrorQuestionIds;
            }

            return null;
        }
    }
}
