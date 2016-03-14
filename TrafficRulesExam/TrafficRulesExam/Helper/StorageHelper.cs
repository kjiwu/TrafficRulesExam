using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Runtime.Serialization.Json;
using TrafficRulesExam.Models;

namespace TrafficRulesExam.Helper
{
    public class StorageHelper
    {
        private static string ImageDirName = "Images";

        private async static Task CreateDir(string folderName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(folder.Path);

            var result = await folder.GetFoldersAsync();
            if (null != result)
            {
                var sf = result.Where(f => f.Name.ToLower().Equals(folderName.ToLower()));
                if (!sf.Any())
                {
                    await folder.CreateFolderAsync(folderName);
                }
            }
        }

        private async static Task<bool> IsFileExist(StorageFolder folder, string fileName)
        {
            if(null == folder || String.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
            var file = files.Where(x => x.Name.ToLower().Equals(fileName.ToLower())).FirstOrDefault();
            return null != file;
        }

        public async static Task<IRandomAccessStream> ConvertStreamToIRandomAccessStream(Stream stream)
        {
            if (null == stream)
            {
                return null;
            }

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            MemoryStream ms = new MemoryStream(buffer);
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var dw = new DataWriter(outputStream);
            dw.WriteBytes(ms.ToArray());
            await dw.StoreAsync();
            var success = await outputStream.FlushAsync();
            if (success)
            {
                return randomAccessStream;
            }
            else
            {
                return null;
            }
        }

        public async static Task<IRandomAccessStream> SaveImageFile(string fileName, byte[] buffer)
        {
            await CreateDir(ImageDirName);

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imageFolder = await folder.GetFolderAsync(ImageDirName);
            if (null != imageFolder)
            {
                StorageFile image = await imageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if (null != image)
                {
                    using (var imageStream = await image.OpenStreamForWriteAsync())
                    {
                        var outputStream = imageStream.AsRandomAccessStream();
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.GifEncoderId, outputStream);
                        using (MemoryStream stream = new MemoryStream(buffer))
                        {
                            IInputStream inputStream = stream.AsInputStream();
                            IRandomAccessStream memStream = new InMemoryRandomAccessStream();
                            var result = await RandomAccessStream.CopyAsync(inputStream, memStream);
                            if (result != 0)
                            {
                                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(memStream);
                                SoftwareBitmap softBmp = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                                encoder.SetSoftwareBitmap(softBmp);
                                await encoder.FlushAsync();
                                await imageStream.FlushAsync();
                                return outputStream;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public async static Task<SoftwareBitmap> GetImageFile(string fileName)
        {
            await CreateDir(ImageDirName);

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imageFolder = await folder.GetFolderAsync(ImageDirName);
            if (null != imageFolder)
            {
                bool result = await IsFileExist(imageFolder, fileName);
                if (result)
                {
                    StorageFile image = await imageFolder.GetFileAsync(fileName);
                    using (IRandomAccessStream stream = await image.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                        SoftwareBitmap softBmp = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                        return softBmp;
                    }
                }
            }

            return null;
        }


        public async static Task<IRandomAccessStream> GetImageFileStream(string fileName)
        {
            await CreateDir(ImageDirName);

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imageFolder = await folder.GetFolderAsync(ImageDirName);
            if (imageFolder != null)
            {
                bool result = await IsFileExist(imageFolder, fileName);
                if (result)
                {
                    StorageFile image = await imageFolder.GetFileAsync(fileName);
                    if (null != image)
                    {
                        return await image.OpenAsync(FileAccessMode.Read);
                    }
                }
            }

            return null;
        }

        #region 保存下载的考试信息

        private static string FilesDirName = "Files";

        public async static void SaveSubjectJsonFile(int subjectId, byte[] buffer)
        {
            await CreateDir(FilesDirName);

            string fileName = subjectId == 1 ? "subject1.json" : "subject4.json";
            var localFolder = ApplicationData.Current.LocalFolder;
            var fileFolder = await localFolder.GetFolderAsync(FilesDirName);
            Stream writeStream = await fileFolder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting);
            writeStream.Write(buffer, 0, buffer.Length);
            await writeStream.FlushAsync();
        }

        public async static Task<Subject> GetSubjectFromJson(int subjectId)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            string path = Path.Combine(localFolder.Path, FilesDirName);
            var fileFolder = StorageFolder.GetFolderFromPathAsync(path);
            if (fileFolder.Status == Windows.Foundation.AsyncStatus.Completed)
            {
                string fileName = subjectId == 1 ? "subject1.json" : "subject4.json";
                bool result = await IsFileExist(fileFolder.GetResults(), fileName);
                if (result)
                {
                    var file = await fileFolder.GetResults().GetFileAsync(fileName);
                    var stream = await file.OpenStreamForWriteAsync();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Subject));
                    return serializer.ReadObject(stream) as Subject;
                }
            }

            return null;
        }

        #endregion
    }
}
