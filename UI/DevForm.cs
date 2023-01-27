using ForgeTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ForgeTools.Core;
using ForgeTools.Core.DataModels;
using Microsoft.VisualBasic.Logging;
using static ForgeTools.ForgeObjectReader;
using Log = Serilog.Log;

namespace DerriksForgeTools
{
    public partial class DevForm : Form
    {
        public DevForm()
        {
            InitializeComponent();
        }

        private void button_gatherItemData_Click(object sender, EventArgs e)
        {
            ForgeAssetCollector.CollectForgeObjectData();
        }
    }
}