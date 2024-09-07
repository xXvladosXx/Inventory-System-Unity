using GoggleImporter.ItemParser;
using UnityEditor;
using UnityEngine;

namespace GoggleImporter
{
    public class ConfigImportsMenu
    {
        private const string SHEET_NAME = "InventoryItems";
        private const string CREDITS_NAME = "###";
        private const string SHEET_ID = "###";

        [MenuItem("GoggleImporter/Import Inventory System")]
        public static async void LoadItemsSettings()
        {
            var sheetsImporter = new GoogleSheetsImporter(CREDITS_NAME, SHEET_ID);

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