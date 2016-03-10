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
using Windows.Storage.Streams;

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

        public static async Task GetExam(int subjectId = 1, Action<Subject> handler = null)
        {
            HttpClient client = new HttpClient();
            await Task.Run(async () =>
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(String.Format(URLFormatter.SubjectUrlFormatter, subjectId));
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Subject));
                        Subject subject = serializer.ReadObject(stream) as Subject;
                        if (null != subject)
                        {
                            if (null != handler)
                            {
                                handler(subject);
                            }
                        }
                    }
                }
                catch
                {

                }
            });
        }

        public async static Task<byte[]> GetQuestionImage(int subjectId, int questionId)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(String.Format(URLFormatter.SubjectImageUrlFormatter, questionId));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }

            return null;
        }
    }
}
