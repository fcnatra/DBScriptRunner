using System;
using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public class CommonDialogs
    {
        internal static string SelectFileDialogBox(string defaultFileName, string initialDirectory = "")
        {
            return ShowFileDialog(new OpenFileDialog(), defaultFileName, initialDirectory);
        }

        internal static string SaveToFileDialogBox(string defaultFileName, string initialDirectory = "")
        {
            return ShowFileDialog(new SaveFileDialog(), defaultFileName, initialDirectory);
        }

        internal static DialogResult AreYouSure(string message = "Are you sure?", string dialogTitle = "PLEASE CONFIRM")
        {
            return MessageBox.Show(message, dialogTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        internal static string ShowFileDialog(FileDialog fileDialog, string defaultFileName, string initialDirectory)
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

        internal static void TellUserToSelectItemsInOrderToMove()
        {
            MessageBox.Show("Select items from the list in order to operate with them", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void TellUserFileCouldNotBeOpened(string fileName)
        {
            MessageBox.Show($"File could not be opened\n{fileName}", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void TellUserToSelectJustOneItem()
        {
            MessageBox.Show("Please select an item (only one) in order to proceed", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
