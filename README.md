# Goggle Inventory Importer

## Overview
**Goggle Inventory Importer** is a Unity tool to import and parse Google Sheets data into a Unity project. It is primarily designed for game settings like inventory items and properties, utilizing the **Google Sheets API**. The project parses Google Sheets and imports inventory data directly into your game settings.

---

## Features
- **Imports inventory items** from a Google Sheets document.
- Supports **parsing of properties**, including equippable items and constant stats.
- Uses **reflection** to dynamically load parsers.
- Integrated with **Google Sheets API** for async data download.

---

## Requirements
- **Unity 2020.3** or later.
- **Google Sheets API** credentials.
- Basic knowledge of Unity Editor scripting.

---

## Setup

1. Clone the repository or download the ZIP file.
2. Place your **Google Sheets API credentials** in the project. Replace the path in the script:
    ```csharp
    var sheetsImporter = new GoogleSheetsImporter(CREDITS_NAME, SHEET_ID);
    ```
3. Set your **Sheet ID** and **Credentials** in the `ConfigImportsMenu.cs` file.

4. Make sure the game settings asset file exists:
    ```csharp
    private const string GAME_SETTINGS_DATA = "Assets/Data/Game Settings.asset";
    ```

5. To trigger the import, go to Unity Editor and click:
    **GoggleImporter > Import Inventory System**.

---

## Code Structure

### 1. **ConfigImportsMenu.cs**
   This class manages the import process via Unity Editor's menu options. It imports both **inventory items** and **property names** from Google Sheets.
   
   ```csharp
   [MenuItem("GoggleImporter/Import Inventory System")]
   public static async void LoadItemsSettings()
  ```
### 2. **GoogleSheetsImporter.cs**
Handles the connection to the Google Sheets API, downloading and parsing sheet data.

```csharp
public async Task DownloadAndParseSheetAsync(string sheetName, IGoogleSheetParser googleSheetParser, int rowIncrement = 1)
```

3. ItemSettingsParser.cs
Parses each row of the sheet into the game’s ItemSettings using dynamic parsers based on the header type.

```csharp
public void ParseSheet(List<string> headers, IList<object> tokens)
```
### 4. Property Parsers

- `EquippablePropertyParser.cs`: Parses items that can be equipped (e.g., weapons, armor).
- `ConstantStatPropertyParser.cs`: Parses constant stat items (e.g., health, strength).

---

## How It Works

1. The importer connects to a Google Sheets document using the Google Sheets API.
2. Based on the sheet name, it pulls the data and applies it to game settings.
3. Reflection is used to dynamically assign the appropriate parser based on the column headers in the sheet.
4. The parsed data is stored in Unity asset files for immediate use in the game.

---

## How to Use

### Step 1: Create Property Types
Define your property types in the `PropertyType` enum:

```csharp
public enum PropertyType 
{
    ConstantStatProperty,
    EquippableProperty
}
```

### Step 2: Create Property Actions
Define your property types in the `ActionType` enum:

```csharp
public enum ActionType 
{
   ConsumableAction,
   EquippableAction
}
```

### Step 3: Create Property Parser
Create a property parser that inherits from `BaseParser` and implements `IPropertySetter`. This allows values to be set from Google Sheets and not just from the editor:

```csharp
public class EquippablePropertyParser : BaseParser, IPropertySetter
{
    public override string PropertyType => InventorySystem.Items.Types.PropertyType.EquippableProperty.ToString();

    public override void Parse(string token, ItemSettings itemSettings)
    {
        if (string.IsNullOrEmpty(token)) return;

        var propertyParts = token.Split(';');
        if (propertyParts.Length < 1)
        {
            Debug.LogError($"Invalid format for EquippableProperty: {token}");
            return;
        }

        var propertyValue = propertyParts[0];

        var property = new EquippableProperty
        {
            EquipType = (EquipType)Enum.Parse(typeof(EquipType), propertyValue)
        };

        if (itemSettings.CurrentType.HasValue)
        {
            itemSettings.EquipTypes.Add(new TypeToEquip
            {
                ActionType = itemSettings.CurrentType.Value,
                EquipProperty = property
            });
        }
        else
        {
            Debug.LogError("No type set for EquippableProperty. Item " + itemSettings.Name);
        }
    }

    public void Set(ActionType actionType, Property property, Item item)
    {
        if (property is EquippableProperty equippableProperty)
        {
            if (!item.Properties.TryGetValue(actionType, out var propertiesList))
            {
                propertiesList = new List<Property>();
                item.Properties[actionType] = propertiesList;
            }

            propertiesList.Add(equippableProperty);
        }
    }
}
```

### Step 4: Create Property Class
Create a property class that inherits from `Property` and set the property parser:

```csharp
public class EquippableProperty : Property
{
    public EquipType EquipType;
    public override IPropertySetter PropertySetter { get; } = new EquippablePropertyParser();
}
```

### Step 5: Create a Type Wrapper
Define a class that acts as a wrapper for your property type:

```csharp
[Serializable]
public class TypeToEquip : IPropertyWithType
{
    [field: SerializeField] public ActionType ActionType { get; set; }
    [field: SerializeField] public EquippableProperty EquipProperty { get; set; }
    public Property Property => EquipProperty;
}
```

### Step 6: Create ItemSettings Class
Define your `ItemSettings` class to hold the properties:

```csharp
[Serializable]
public class ItemSettings
{
    public string Name;
    public bool IsStackable;
    public int MaxInStack;

    public PropertyType? CurrentType { get; private set; }

    public List<TypeToConstantStat> ConstantStats = new List<TypeToConstantStat>();
    public List<TypeToEquip> EquipTypes = new List<TypeToEquip>();

    public void SetCurrentType(PropertyType propertyType)
    {
        CurrentType = propertyType;
    }
}
```

## Example Sheet Structure

For the importer to work correctly, your Google Sheet should have the following structure:

![image](https://github.com/user-attachments/assets/3eb64731-b56a-4ad6-aa9e-3ebd0a2fd472)
![image](https://github.com/user-attachments/assets/410cb02b-b780-4265-b510-46f24f9b933a)

- **Item Name**: The name of the item.
- **Is Stackable**: The stack of the item.
- **Stack Size**: The size of the stack.
- **Type**: Specifies the property type (e.g., Equippable, Consumable).
- **EquipType**: Only used for equippable items, specifies the type of equipment.
- **StatType**: The stat affected by the item.
- **Value**: The stat's value (for consumable items).

---

## Contributing

Feel free to open issues or submit pull requests for new features or bug fixes. Contributions are welcome!
