using System.Numerics;
using Serilog;

namespace DerriksForgeTools;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }


    private List<string> directoryFiles = new List<string>();
    private Dictionary<Button, string> fileButtons = new Dictionary<Button, string>();

    private void DisplayMapData(string xmlPath)
    {
        // ADD Displaying of map data here. mostly for testing
        Log.Debug("diplay map data method {XMLPath}", xmlPath);
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

            directoryFiles = Directory.GetFiles(directoryDialog.SelectedPath, "*.xml").ToList();

            foreach (var file in directoryFiles)
            {
                Button b = new Button();
                b.Text = Path.GetFileName(file);
                fileButtons.Add(b, file);
                b.Click += new EventHandler(delegate(object? o, EventArgs args)
                {
                    DisplayMapData(fileButtons[(Button)o]);
                });

                FileListLayoutPanel.Controls.Add(b);
            }
        }
    }
}