using System;
using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public class CommonDialogs
    {
        public static string SelectFileDialogBox(string defaultFileName, string initialDirectory = "")
        {
            return ShowFileDialog(new OpenFileDialog(), defaultFileName, initialDirectory);
        }

        public static string SaveToFileDialogBox(string defaultFileName, string initialDirectory = "")
        {
            return ShowFileDialog(new SaveFileDialog(), defaultFileName, initialDirectory);
        }

        public static DialogResult AreYouSure(string dialogTitle = "PLEASE CONFIRM")
        {
            return MessageBox.Show("Are you sure?", dialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private static string ShowFileDialog(FileDialog fileDialog, string defaultFileName, string initialDirectory)
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

        internal static void TellUserToSelectJustOneItem()
        {
            MessageBox.Show("Please select an item (only one) in order to proceed", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
