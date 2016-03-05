using SQLitePCL.pretty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone;
using System.IO;
using System.Diagnostics;
using TrafficRulesExam.Models;
using Windows.Storage.Streams;

namespace TrafficRulesExam.Helper
{
    internal class DatabaseHelper
    {
        class Database
        {
            public static string DatabaseName
            {
                get
                {
                    string path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    return Path.Combine(path, "traffic_rules.sqlite");
                }
            }

            public static string TableName
            {
                get
                {
                    return "questions";
                }
            }

            public static string CreateTableStatement
            {
                get
                {
                    string sql = "create table if not exists {0}(sectionId integer, id integer primary key, question text, " + 
                                 "image integer, options text, answer text, explain text, picture blob)";
                    return String.Format(sql, TableName);
                }
            }
        }

        public static void CreateDatabase()
        {
            Debug.WriteLine(Database.DatabaseName);
            using (SQLiteDatabaseConnection connection = SQLite3.Open(Database.DatabaseName))
            {
                connection.Execute(Database.CreateTableStatement);
            }            
        }

        public static void InsertQuestions(List<QuestionItem> questions)
        {
            if(null == questions)
            {
                return;
            }

            CreateDatabase();

            using (SQLiteDatabaseConnection connection = SQLite3.Open(Database.DatabaseName))
            {
                Debug.WriteLine("begin insert");
                connection.RunInTransaction((conn) =>
                {
                    foreach (QuestionItem question in questions)
                    {
                        Stream image = null;

                        string sql = "insert or replace into {0}(sectionId, id, question, image, options, answer, explain, picture)" +
                                     " values({1}, {2}, '{3}', {4}, '{5}', '{6}', '{7}', ?)";
                        sql = String.Format(sql, Database.TableName,
                                                              question.SectionId,
                                                              question.Id,
                                                              FormatString(question.Question),
                                                              question.Image,
                                                              GetArrayString<string>(question.Options),
                                                              GetArrayString<int>(question.Answer),
                                                              FormatString(question.Explain));

                        conn.Execute(sql, image);
                    }
                });
            }
        }

        public static void InsertQuestionPicture(int questionId, Stream stream)
        {
            if(questionId < 0 || null == stream)
            {
                return;
            }

            CreateDatabase();

            using (SQLiteDatabaseConnection connection = SQLite3.Open(Database.DatabaseName))
            {
                string sql = "update {0} set picture=? where id={1}";
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                sql = String.Format(sql, Database.TableName, questionId);
                connection.Execute(sql, buffer);
            }
        }

        public async static Task<IRandomAccessStream> GetQuestionImage(int questionId)
        {
            IRandomAccessStream randomAccessStream = null;

            if (questionId < 0)
            {
                return randomAccessStream;
            }

            CreateDatabase();

            using (SQLiteDatabaseConnection connection = SQLite3.Open(Database.DatabaseName))
            {
                string sql = "select picture from {0} where id={1}";
                sql = String.Format(sql, Database.TableName, questionId);
                var result = connection.Query(sql);
                if (null != result)
                {
                    foreach(var item in result)
                    {
                        IResultSetValue value = item[0];
                        byte[] buffer = value.ToBlob();
                        MemoryStream stream = new MemoryStream(buffer);
                        randomAccessStream = new InMemoryRandomAccessStream();
                        var outputStream = randomAccessStream.GetOutputStreamAt(0);
                        var dw = new DataWriter(outputStream);
                        var task = new Task(() => dw.WriteBytes(stream.ToArray()));
                        task.Start();
                        await task;
                        await dw.StoreAsync();
                        var success = await outputStream.FlushAsync();
                        return randomAccessStream;
                    }
                }
            }

            return randomAccessStream;
        }

        public static List<QuestionItem> GetLocalQuestions()
        {
            List<QuestionItem> questions = new List<QuestionItem>();
            CreateDatabase();

            using (SQLiteDatabaseConnection connection = SQLite3.Open(Database.DatabaseName))
            {
                var items = connection.Query("select * from " + Database.TableName);
                foreach(var item in items)
                {
                    QuestionItem question = new QuestionItem();
                    question.SectionId = item[0].ToInt();
                    question.Id = item[1].ToInt();
                    question.Question = item[2].ToString();
                    question.Image = item[3].ToInt();
                    question.Options = GetArray<string>(item[4].ToString());
                    question.Answer = GetArray<int>(item[5].ToString());
                    question.Explain = item[6].ToString();
                    questions.Add(question);
                }
            }

            return questions;
        }

        private static string GetArrayString<T>(List<T> array)
        {
            string result = String.Empty;

            foreach(T value in array)
            {
                result += FormatString(value.ToString()) + "|";
            }

            if (!String.IsNullOrEmpty(result))
            {
                result = result.Remove(result.Length - 1);
            }

            return result;
        }

        private static List<T> GetArray<T>(string value)
        {
            List<T> result = new List<T>();

            if (!String.IsNullOrEmpty(value))
            {
                string[] items = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string type = typeof(T).Name;
                    switch (type)
                    {
                        case "Int32":
                            result.Add((T)(Object)Convert.ToInt32(item));
                            break;
                        case "String":
                            result.Add((T)(item as object));
                            break;
                    }
                    
                }
            }

            return result;
        }

        private static string FormatString(string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            
            string result = value;
            result = result.Replace("'", "''");
            return result;
        }
    }
}
