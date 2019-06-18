using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace DbScriptRunner.Services
{
    public class ApplicationStatusBackup
    {
        private string _statusBackupFileName = "AppStatusBackup.txt";
        private string _lastOpenedFilesFileName = "LastOpenedFiles.txt";

        public List<string> BackupContent = new List<string>();
        
        public void SaveStatus()
        {
            SaveContentToIsolatedStorage(BackupContent, _statusBackupFileName);
        }

        public void RecoverStatus()
        {
            var rowsInContent = LoadContentFromIsolatedStorage(_statusBackupFileName);
            BackupContent.Clear();
            BackupContent.AddRange(rowsInContent);
        }

        public IEnumerable<string> RecoverLastOpenedFiles()
        {
            var rowsInContent = LoadContentFromIsolatedStorage(_lastOpenedFilesFileName);
            return rowsInContent.ToList();
        }

        public void SaveLastOpenedFiles(IEnumerable<string> fileList)
        {
            SaveContentToIsolatedStorage(fileList, _lastOpenedFilesFileName);
        }

        private void SaveContentToIsolatedStorage(IEnumerable<string> content, string fileName)
        {
            var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            var isoStream = isoFile.OpenFile(fileName, System.IO.FileMode.Create);
            foreach (var data in content)
            {
                var buffer = Encoding.Default.GetBytes(data);
                isoStream.Write(buffer, 0, data.Length);
                isoStream.WriteByte(13);
            }
            isoStream.Close();
            isoFile.Close();
        }

        private IEnumerable<string> LoadContentFromIsolatedStorage(string isoFileName)
        {
            var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            var isoStream = isoFile.OpenFile(isoFileName, System.IO.FileMode.OpenOrCreate);
            var bufferLength = isoFile.UsedSize;
            var buffer = new byte[bufferLength];

            isoStream.Read(buffer, 0, (int)bufferLength);
            isoStream.Close();
            isoFile.Close();

            string content = Encoding.Default.GetString(buffer);
            var rowsInContent = content.Split(new char[] { (char)13 });
            return rowsInContent.Where(row => !string.IsNullOrEmpty(row) && !row.StartsWith("\0"));
        }
    }
}
