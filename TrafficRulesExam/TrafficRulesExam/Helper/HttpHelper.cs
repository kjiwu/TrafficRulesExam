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
using Windows.Storage;

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
            var subject = await StorageHelper.GetSubjectFromJson(subjectId);

            if (null == subject)
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.GetAsync(String.Format(URLFormatter.SubjectUrlFormatter, subjectId));
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream stream = await response.Content.ReadAsStreamAsync();
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);

                            stream.Seek(0, 0);
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Subject));
                            subject = serializer.ReadObject(stream) as Subject;

                            StorageHelper.SaveSubjectJsonFile(subjectId, buffer);

                            if (null != subject)
                            {
                                if (null != handler)
                                {
                                    handler(subject);
                                }
                            }
                        }
                        else
                        {
                            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(String.Format("ms-appx:///Assets/subject{0}.json", subjectId)));
                            var stream = await file.OpenReadAsync();
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Subject));
                            subject = serializer.ReadObject(stream.AsStream()) as Subject;
                            if (null != subject)
                            {
                                if (null != handler)
                                {
                                    handler(subject);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);

                        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(String.Format("ms-appx:///Assets/subject{0}.json", subjectId)));
                        var stream = await file.OpenReadAsync();
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Subject));
                        subject = serializer.ReadObject(stream.AsStream()) as Subject;
                        if (null != subject)
                        {
                            if (null != handler)
                            {
                                handler(subject);
                            }
                        }
                    }
                });
            }
            else
            {
                if (null != handler)
                {
                    handler(subject);
                }
            }           
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

                await StorageHelper.SaveImageFile(String.Format("_{0}.gif", questionId), buffer);

                return buffer;
            }

            return null;
        }
    }
}
