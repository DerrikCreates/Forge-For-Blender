using System.Windows.Forms;

namespace ForgeTools;

public class Helper
{
    public static bool SelectFolderDialog(out string selectedPath)
    {
        FolderBrowserDialog fileDialog = new FolderBrowserDialog();



        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            selectedPath = fileDialog.SelectedPath;
            return true;
        }

        selectedPath = "";
        return false;
    }
}