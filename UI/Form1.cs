using System.Numerics;
using Serilog;

namespace DerriksForgeTools;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Quaternion q = Quaternion.CreateFromYawPitchRoll(0, 90, 0);


        Vector3 vector = new Vector3(0, 0, 1);


        Log.Information("{ForwardVector} , {Length}", Vector3.Transform(vector, q), vector.Length());
    }


    private void button1_Click(object sender, EventArgs e)
    {
    }
}