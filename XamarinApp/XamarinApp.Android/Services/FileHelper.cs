using System.IO;
using XamarinApp.Droid.Services;
using XamarinApp.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace XamarinApp.Droid.Services
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}