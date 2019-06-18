using System.Windows.Forms;

namespace DbScriptRunner.UI
{
    public class CommonDialogs
    {
        public static string SelectFileDialogBox()
        {
            string fullPath = null;

            var openFileDialog = new OpenFileDialog();
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                fullPath = openFileDialog.FileName;
            }
            return fullPath;
        }

        public static void TellUserToSelectItemsInOrderToMove()
        {
            MessageBox.Show("Select items from the list in order to operate with them", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
