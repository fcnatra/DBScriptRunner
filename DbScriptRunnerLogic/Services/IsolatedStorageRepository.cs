using DbScriptRunnerLogic.Interfaces;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace DbScriptRunnerLogic.Services
{
    public class IsolatedStorageRepository : IRepository
    {
        public string Name { get; set; }

        // Not used on this implementation
        public string Location { get; set; }

        public string Load()
        {
            var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            var isoStream = isoFile.OpenFile(Name, System.IO.FileMode.OpenOrCreate);
            var bufferLength = isoFile.UsedSize;
            var buffer = new byte[bufferLength];

            isoStream.Read(buffer, 0, (int)bufferLength);
            isoStream.Close();
            isoFile.Close();

            string content = Encoding.Default.GetString(buffer);
            return content;
        }

        public void Save(string content)
        {
            var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            var isoStream = isoFile.OpenFile(Name, FileMode.Create);
            var buffer = Encoding.Default.GetBytes(content);
            isoStream.Write(buffer, 0, content.Length);

            isoStream.Close();
            isoFile.Close();
        }
    }
}
