using System.IO;
using UnityEditor;

namespace GoggleImporter.PropertyParser
{
    public static class EnumGenerator
    {
        private const string ENUM_FILE_PATH = "Assets/Scripts/InventorySystem/Items/Properties/PropertyName.cs";
        private const string ENUM_NAMESPACE = "InventorySystem.Items.Properties";

        public static void GenerateEnum(string[] values)
        {
            using (StreamWriter streamWriter = new StreamWriter(ENUM_FILE_PATH))
            {
                streamWriter.WriteLine($"namespace {ENUM_NAMESPACE}");
                streamWriter.WriteLine("{");

                streamWriter.WriteLine("\tpublic enum PropertyName");
                streamWriter.WriteLine("\t{");

                for (int i = 0; i < values.Length; i++)
                {
                    string value = values[i];
                    streamWriter.WriteLine($"\t\t{value},");
                }

                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}"); 
            }

            AssetDatabase.ImportAsset(ENUM_FILE_PATH);
            AssetDatabase.Refresh();
        }
    }
}