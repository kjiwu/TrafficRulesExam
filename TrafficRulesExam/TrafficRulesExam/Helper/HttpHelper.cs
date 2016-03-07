using System;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using TrafficRulesExam.Models;
using System.IO;
using System.Threading;
using Windows.System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TrafficRulesExam.Helper
{
    internal class HttpHelper
    {
        class URLFormatter
        {
            public static string SubjectUrlFormatter
            {
                get
                {
                    return "http://fapei.yunchelife.com/Treasury/dummy/subject{0}.json";
                }
            }

            public static string SubjectImageUrlFormatter
            {
                get
                {
                    return "http://fapei.yunchelife.com/Treasury/img/questionImgs/_{0}.gif";
                }
            }

            public static string ImageFileNameFormatter
            {
                get
                {
                    return "_{0}.gif";
                }
            }
        }

        public static async void GetExam(int subjectId = 1, Action<Subject> handler = null)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(String.Format(URLFormatter.SubjectUrlFormatter, subjectId));
            if(response.StatusCode == HttpStatusCode.OK)
            {                
                Stream stream = await response.Content.ReadAsStreamAsync();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Subject));
                Subject subject = serializer.ReadObject(stream) as Subject;
                if(null != subject)
                {
                    await ThreadPool.RunAsync((workItem) => {
                        DatabaseHelper.InsertQuestions(subjectId, subject.Exam.Questions);
                    });                    

                    if(null != handler)
                    {
                        handler(subject);
                    }
                }
            }
        }

        public static async Task GetQuestionImage(int subjectId, int questionId, Action completed)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(String.Format(URLFormatter.SubjectImageUrlFormatter, questionId));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                DatabaseHelper.InsertQuestionPicture(subjectId, questionId, stream);
                StorageHelper.SaveImageFile(String.Format(URLFormatter.ImageFileNameFormatter, questionId), stream, completed);
            }
        }
    }
}
