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
            var streamReader = new StreamReader(FullRepositoryName);
            var data = streamReader.ReadToEnd();
            streamReader.Close();
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
