using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public class CommonDialogs
    {
        public static string SelectFileDialogBox(string defaultFileName)
        {
            return ShowDialog(new OpenFileDialog() { FileName = defaultFileName });
        }

        public static string SaveToFileDialogBox(string defaultFileName)
        {
            return ShowDialog(new SaveFileDialog() { FileName = defaultFileName });
        }

        private static string ShowDialog(FileDialog fileDialog)
        {
            string fullPath = null;
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
