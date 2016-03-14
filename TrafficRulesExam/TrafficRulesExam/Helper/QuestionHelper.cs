using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.Threading.Tasks;
using System;

namespace TrafficRulesExam.Helper
{
    public class QuestionHelper
    {
        public async static Task<IRandomAccessStream> GetQuestionImage(int subjectId, int questionId)
        {
            IRandomAccessStream imageStream = await StorageHelper.GetImageFileStream(String.Format("_{0}.gif", questionId));
            if(null == imageStream)
            {
                var buffer = await HttpHelper.GetQuestionImage(subjectId, questionId);
                MemoryStream ms = new MemoryStream(buffer);
                var randomAccessStream = new InMemoryRandomAccessStream();
                var outputStream = randomAccessStream.GetOutputStreamAt(0);
                var dw = new DataWriter(outputStream);
                var task = new Task(() => dw.WriteBytes(ms.ToArray()));
                task.Start();
                await task;
                await dw.StoreAsync();
                var success = await outputStream.FlushAsync();
                return randomAccessStream;
            }

            return imageStream;
        }
    }
}
