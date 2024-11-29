using GoggleImporter.ItemParser;
using GoggleImporter.PropertyParser;
using UnityEditor;
using UnityEngine;

namespace GoggleImporter
{
    public class ConfigImportsMenu
    {
        private const string ITEMS_SHEET_NAME = "InventoryItems";
        private const string PROPERTIES_NAME_SHEET_NAME = "PropertyName";
        private const string CREDITS_NAME = "inventorysystem-434520-4c0545b0de51.json";
        private const string SHEET_ID = "1wXHYN_RUZPmRagWtJmsx3uC8D7I81f0jgoSTnwycACM";
        private const string GAME_SETTINGS_DATA = "Assets/Data/Game Settings.asset";

        [MenuItem("GoggleImporter/Import Inventory System")]
        public static async void LoadItemsSettings()
        {
            var sheetsImporter = new GoogleSheetsImporter(CREDITS_NAME, SHEET_ID);

            var gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>(GAME_SETTINGS_DATA);
            var itemsParser = new ItemSettingsParser(gameSettings);
            await sheetsImporter.DownloadAndParseSheetAsync(ITEMS_SHEET_NAME, itemsParser, 2);
            
            //var jsonForSaving = JsonUtility.ToJson(gameSettings, true);
            //Debug.Log(jsonForSaving);

            gameSettings.UpdateItems();
            
            EditorUtility.SetDirty(gameSettings);
            AssetDatabase.SaveAssets();
        } 
        
        [MenuItem("GoggleImporter/Import Property Names")]
        public static async void LoadPropertyNames()
        {
            var sheetsImporter = new GoogleSheetsImporter(CREDITS_NAME, SHEET_ID);

            var gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>(GAME_SETTINGS_DATA);
            
            var propertyNamesParser = new PropertyNameParser(gameSettings);
            await sheetsImporter.DownloadAndParseSheetAsync(PROPERTIES_NAME_SHEET_NAME, propertyNamesParser);
            gameSettings.UpdatePropertyNames();

            EditorUtility.SetDirty(gameSettings);
            AssetDatabase.SaveAssets();
        }
    }
}