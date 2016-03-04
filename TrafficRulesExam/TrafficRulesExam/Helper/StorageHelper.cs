using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace TrafficRulesExam.Helper
{
    public class StorageHelper
    {
        private static string ImageDirName = "Images";

        private async static Task CreateImageDir()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(folder.Path);

            var result = await folder.GetFoldersAsync();
            if(null != result)
            {
                var sf = result.Where(f => f.Name.ToLower().Equals(ImageDirName.ToLower()));
                if (!sf.Any())
                {
                    await folder.CreateFolderAsync(ImageDirName);
                }
            }
        }

        public async static void SaveImageFile(string fileName, Stream stream, Action completed)
        {
            await CreateImageDir();

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imageFolder = await folder.GetFolderAsync(ImageDirName);
            if(null != imageFolder)
            {
                StorageFile image = await imageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if(null != image)
                {
                    using (var imageStream = await image.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.GifEncoderId, imageStream);
                        IInputStream inputStream = stream.AsInputStream();
                        IRandomAccessStream memStream = new InMemoryRandomAccessStream();
                        await RandomAccessStream.CopyAsync(inputStream, memStream);
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(memStream);
                        SoftwareBitmap softBmp = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                        encoder.SetSoftwareBitmap(softBmp);
                        await encoder.FlushAsync();
                        await imageStream.FlushAsync();

                        WriteableBitmap bitmap = new WriteableBitmap(0, 0);
                        bitmap.SetSource(memStream);          
                        
                        if(null != completed)
                        {
                            completed();
                        }             
                    }
                }
            }            
        }

        public async static Task<SoftwareBitmap> GetImageFile(string fileName)
        {
            await CreateImageDir();

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder imageFolder = await folder.GetFolderAsync(ImageDirName);
            if (null != imageFolder)
            {
                StorageFile image = await imageFolder.GetFileAsync(fileName);
                using (IRandomAccessStream stream = await image.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap softBmp = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    return softBmp;
                }
            }

            return null;
        }
    }
}
