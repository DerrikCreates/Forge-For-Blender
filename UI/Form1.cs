using System.Numerics;
using Serilog;

namespace DerriksForgeTools;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }


    private void button1_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog directoryDialog = new FolderBrowserDialog();
        directoryDialog.InitialDirectory = Program.Settings.LastUsedPath;
        

        if (directoryDialog.ShowDialog() == DialogResult.OK)
        {
            var fileInfo = new FileInfo(directoryDialog.SelectedPath);
            Program.Settings.LastUsedPath = directoryDialog.SelectedPath;
            Log.Information(directoryDialog.SelectedPath);
            
        }
    }
}