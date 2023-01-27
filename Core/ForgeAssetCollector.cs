using ForgeTools.Core.DataModels;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace ForgeTools.Core;

public static class ForgeAssetCollector
{

    public static void CollectForgeObjectData()
    {
        
        string folderPath;
        if (Helper.SelectFolderDialog(out folderPath))
        {
            string[] files = System.IO.Directory.GetFiles(folderPath, "*.forgeobjectdata");

            List<ForgeAssetData> data = new List<ForgeAssetData>();
            foreach (var item in files)
            {
                var obj = ForgeObjectReader.ReadForgeObjectFile(item);

                if (obj != null)
                {
                    data.Add(obj);
                        
                }

                    
                    
            }

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(folderPath + "/ItemData.json", json);
        }
    }
}