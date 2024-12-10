using GoggleImporter.Runtime;
using GoggleImporter.Runtime.ItemParser.Parsers;
using UnityEditor;

namespace Example.InventorySystem.Configs.Editor
{
    public class ConfigImportsMenu
    {
        private const string ITEMS_SHEET_NAME = "InventoryItems";
        private const string CREDITS_NAME = "inventorysystem-434520-4c0545b0de51.json";
        private const string SHEET_ID = "1wXHYN_RUZPmRagWtJmsx3uC8D7I81f0jgoSTnwycACM";
        private const string GAME_SETTINGS_DATA = "Assets/Data/Game Settings.asset";

        [MenuItem("GoggleImporter/Import Inventory System")]
        public static async void LoadItemsSettings()
        {
            var sheetsImporter = new GoogleSheetsImporter(CREDITS_NAME, SHEET_ID);

            var gameSettings = AssetDatabase.LoadAssetAtPath<GameSettings>(GAME_SETTINGS_DATA);
            var itemsParser = new ItemDataParser<ItemParsableData>(gameSettings.Items);
            await sheetsImporter.DownloadAndParseSheetAsync(ITEMS_SHEET_NAME, itemsParser, 2);
            
            gameSettings.UpdateItems(itemsParser);
            
            EditorUtility.SetDirty(gameSettings);
            AssetDatabase.SaveAssets();
        } 
    }
}