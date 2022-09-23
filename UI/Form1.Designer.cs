namespace DerriksForgeTools;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.button1 = new System.Windows.Forms.Button();
            this.FileListLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.label_ActiveFilePath = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_MapName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_LoadBlenderJson = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "Choose folder with xml map files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FileListLayoutPanel
            // 
            this.FileListLayoutPanel.Location = new System.Drawing.Point(12, 73);
            this.FileListLayoutPanel.Name = "FileListLayoutPanel";
            this.FileListLayoutPanel.Size = new System.Drawing.Size(162, 365);
            this.FileListLayoutPanel.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(182, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 55);
            this.button2.TabIndex = 3;
            this.button2.Text = "Test Button";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label_ActiveFilePath
            // 
            this.label_ActiveFilePath.Location = new System.Drawing.Point(276, 21);
            this.label_ActiveFilePath.Name = "label_ActiveFilePath";
            this.label_ActiveFilePath.Size = new System.Drawing.Size(512, 21);
            this.label_ActiveFilePath.TabIndex = 4;
            this.label_ActiveFilePath.Text = "file path and name";
            this.label_ActiveFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(276, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(512, 365);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_MapName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 55);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label_MapName
            // 
            this.label_MapName.BackColor = System.Drawing.Color.Transparent;
            this.label_MapName.Location = new System.Drawing.Point(52, 19);
            this.label_MapName.Name = "label_MapName";
            this.label_MapName.Size = new System.Drawing.Size(97, 33);
            this.label_MapName.TabIndex = 1;
            this.label_MapName.Text = "Map Name";
            this.label_MapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 33);
            this.label2.TabIndex = 0;
            this.label2.Text = "Map:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(276, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(512, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Loaded Map Data";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_LoadBlenderJson
            // 
            this.button_LoadBlenderJson.Location = new System.Drawing.Point(276, 12);
            this.button_LoadBlenderJson.Name = "button_LoadBlenderJson";
            this.button_LoadBlenderJson.Size = new System.Drawing.Size(95, 55);
            this.button_LoadBlenderJson.TabIndex = 6;
            this.button_LoadBlenderJson.Text = "Load Blender Json";
            this.button_LoadBlenderJson.UseVisualStyleBackColor = true;
            this.button_LoadBlenderJson.Click += new System.EventHandler(this.button_LoadBlenderJson_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_LoadBlenderJson);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label_ActiveFilePath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileListLayoutPanel);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private OpenFileDialog openFileDialog1;
    private Button button1;
    private FlowLayoutPanel FileListLayoutPanel;
    private Button button2;
    private Label label_ActiveFilePath;
    private GroupBox groupBox1;
    private Label label1;
    private GroupBox groupBox2;
    private Label label_MapName;
    private Label label2;
    private Button button_LoadBlenderJson;
}