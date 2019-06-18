using DbScriptRunnerLogic.Interfaces;
using System.IO;
using System.Text;

namespace DbScriptRunnerLogic.Services
{
    public class TextFileRepository : IRepository
    {
        public string Name { get; set; }
        public string Location { get; set; }

        private string FullRepositoryName { get { return Path.Combine(Location, Name); } }

        public string Load()
        {
            var streamReader = new System.IO.StreamReader(FullRepositoryName);
            var data = streamReader.ReadToEnd();
            streamReader.Close();
            return data;
        }

        public string Load(Stream fileStream)
        {
            int bufferSize = 1024 * 1000;
            var buffer = new byte[bufferSize];

            int bytesRead = fileStream.Read(buffer, 0, bufferSize);
            fileStream.Close();

            var data = Encoding.Default.GetString(buffer); ;
            return data;
        }

        public void Save(string content)
        {
            var streamWriter = new StreamWriter(FullRepositoryName);
            streamWriter.WriteLine(content);
            streamWriter.Close();
        }
    }
}
