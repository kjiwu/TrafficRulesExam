using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;
using Windows.Storage;

namespace TrafficRulesExam.Helper
{
    public class StorageHelper
    {
        public async static void CreateImageDir()
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            string path = folder.Path;
            Debug.WriteLine(path);
            string dir = Path.Combine(path, "images");
            var result = await folder.GetFoldersAsync();
            if(null != result)
            {
                var sf = result.Where(f => f.Name.Equals("images"));
                if (!sf.Any())
                {
                    await folder.CreateFolderAsync("images");
                }
            }
        }
    }
}
