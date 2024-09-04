using UnityEditor;
using UnityEngine;

namespace GoggleImporter
{
    public class ConfigImportsMenu
    {
        private const string SHEET_NAME = "InventoryItems";

        [MenuItem("GoggleImporter/Import Inventory System")]
        public static async void LoadItemsSettings()
        {
            var sheetsImporter = new GoogleSheetsImporter("Your JSON", "Your link");

            var gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/Data/Game Settings.asset");
            var itemsParser = new ItemSettingsParser(gameSettings);
            await sheetsImporter.DownloadAndParseSheetAsync(SHEET_NAME, itemsParser);
            
            var jsonForSaving = JsonUtility.ToJson(gameSettings, true);
            Debug.Log(jsonForSaving);

            gameSettings.UpdateItems();
            
            EditorUtility.SetDirty(gameSettings);
            AssetDatabase.SaveAssets();
        }
    }
}