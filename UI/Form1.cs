using System.Numerics;
using Serilog;

namespace DerriksForgeTools;

public partial class Form1 : Form
{
    private Button[] _mapSelectButtons = new Button[10];

    public Form1()
    {
        InitializeComponent();

        for (int i = 0; i < 10; i++)
        {
            var b = new Button();
            b.Name = "Empty";
            FileListLayoutPanel.Controls.Add(b);
            _mapSelectButtons[i] = b;
        }
    }


    private List<string> _directoryFiles = new List<string>();
    private Dictionary<Button, string> _fileButtons = new Dictionary<Button, string>();

    private void DisplayMapData(string xmlPath)
    {
        // ADD Displaying of map data here. mostly for testing
        Log.Debug("display map data method {XMLPath}", xmlPath);
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

            _directoryFiles = Directory.GetFiles(directoryDialog.SelectedPath, "*.xml").ToList();

            foreach (var file in _directoryFiles)
            {
                Button b = new Button();
                b.Text = Path.GetFileName(file);
                _fileButtons.Add(b, file);
                b.Click += new EventHandler(delegate(object? o, EventArgs args)
                {
                    DisplayMapData(_fileButtons[(Button)o]);
                });

                FileListLayoutPanel.Controls.Add(b);
            }
        }
    }
}