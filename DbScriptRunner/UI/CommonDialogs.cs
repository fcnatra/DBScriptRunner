using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public class CommonDialogs
    {
        public static string SelectFileDialogBox(string defaultFileName, string initialDirectory = "")
        {
            return ShowDialog(new OpenFileDialog(), defaultFileName, initialDirectory);
        }

        public static string SaveToFileDialogBox(string defaultFileName, string initialDirectory = "")
        {
            return ShowDialog(new SaveFileDialog(), defaultFileName, initialDirectory);
        }

        private static string ShowDialog(FileDialog fileDialog, string defaultFileName, string initialDirectory)
        {
            string fullPath = null;

            fileDialog.FileName = defaultFileName;
            fileDialog.InitialDirectory = initialDirectory;

            var dialogResult = fileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                fullPath = fileDialog.FileName;
            }
            return fullPath;
        }


        public static void TellUserToSelectItemsInOrderToMove()
        {
            MessageBox.Show("Select items from the list in order to operate with them", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
