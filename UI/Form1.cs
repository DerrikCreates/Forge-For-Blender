using System.Numerics;
using ForgeTools;
using Serilog;

namespace DerriksForgeTools;

public partial class Form1 : Form
{
    private Dictionary<Button, string> fileButtons = new Dictionary<Button, string>();

    public Form1()
    {
        InitializeComponent();
    }

    private string _activeFile;

    private void OnFileButtonClick(object? sender, EventArgs e)
    {
        var path = fileButtons[(Button)sender];

        LoadFile(new FileInfo(path));
    }


    private void DisplayMapData(string xmlPath)
    {
        // ADD Displaying of map data here. mostly for testing
        Log.Debug("display map data method {XMLPath}", xmlPath);
    }

    private void AddFileToButtonLayout(FileInfo path)
    {
        if (fileButtons.ContainsValue(path.FullName) == false)
        {
            var b = new Button();
            b.Text = path.Name;
            FileListLayoutPanel.Controls.Add(b);
            fileButtons.Add(b, path.FullName);
            b.Click += OnFileButtonClick;
        }
    }

    private void LoadFile(FileInfo file)
    {
        Log.Information("Loading File {FileInfo}", file);
        _activeFile = file.FullName;
        label_ActiveFilePath.Text = file.Name;

        var map = Util.BuildMapFromXML(file.FullName);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.InitialDirectory = Program.Settings.LastUsedPath;
        fileDialog.Filter = "xml files (*.xml)|*.xml";

        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            var fileInfo = new FileInfo(fileDialog.FileName);
            Program.Settings.LastUsedPath = fileDialog.FileName;
            Log.Information(fileDialog.FileName);
            AddFileToButtonLayout(fileInfo);
            LoadFile(fileInfo);
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void button2_Click(object sender, EventArgs e)
    {
    }
}