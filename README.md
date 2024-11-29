# Goggle Inventory Importer

**Goggle Inventory Importer** is a Unity tool designed to integrate **Google Sheets** data directly into your Unity project. It provides functionality to parse and import game settings, such as inventory items and their properties, using the **Google Sheets API**. This tool streamlines the setup of game inventories by eliminating the need for manual entry of item data.

## Key Features

- **Imports inventory items** from a Google Sheets document.
- Supports **parsing of properties**, including equippable items and constant stats.
- Uses **reflection** to dynamically load parsers.
- Integrated with **Google Sheets API** for async data download.

---

## Example Usage: `InventoryController`

![image](https://github.com/user-attachments/assets/c9d98e74-6b41-47ff-983a-72c7d4fad191)

The `InventoryController` script showcases advanced inventory management capabilities within Unity.

### Features

- **Inventory Panels**  
  Manages different inventory UI panels, such as:
  - Equipment Panel
  - Inventory Panel
  - Loot Panel

- **Tooltips and Context Menus**  
  Provides item details and action menus based on user interaction.

- **Item Actions**  
  Supports contextual actions like:
  - Equipping
  - Consuming
  - Transferring
  - Unequipping

- **Loot Containers**  
  Handles dynamic loot containers and their associated UI.

- **Stat Collection**  
  Aggregates and applies item stats to the player or other game entities.

- **Event Handling**  
  Centralized management of user interactions, including dragging, clicking, and filtering.

---

### Advanced Features

- **Drag-and-Drop UI**  
  Facilitates smooth drag-and-drop functionality for inventory items.

- **Dynamic Filtering**  
  Supports real-time search and filtering based on:
  - Item types
  - Item names

- **Panels Management**  
  Automatically opens and closes inventory panels, efficiently handling their lifecycle.

- **Modular Design**  
  Actions like equipping and transferring are encapsulated in reusable classes such as:
  - `EquipClickAction`
  - `TransferClickAction`

---

## Requirements
- **Unity 2020.3** or later.
- **Google Sheets API** credentials.
- Basic knowledge of Unity Editor scripting.
- Odin Inspector.

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

## Inventory Importer Code Structure

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

### Step 1: Create Property Actions
`ActionType` is an abstract base class, and specific actions are implemented as derived classes. The use of a custom `ActionTypeAttribute` ensures flexibility for metadata or runtime reflection.

```csharp
[ActionType]
public class EquippableAction : ActionType
{
    public override string ToString() => "After Equip";
}
```

### Step 2: Create Property Class
Create a property class that inherits from `Property` and set the property type with all neccesary data, **remember to set the same name of class as in your sheet`s property**:

```csharp
public class EquippableProperty : Property
{
    public EquipType EquipType;
    public int Level;
    public override string ToString() => string.Empty;
}
```

### Step 3: Create Property Parser
Create a property parser that inherits from `BaseParser`. This allows values to be set from Google Sheets and not just from the editor:

```csharp
public class EquippablePropertyParser : BaseParser, IPropertySetter
{
    public override string PropertyType => nameof(EquippableProperty);

    public override void Parse(string token, ItemSettings itemSettings)
    {
        if (string.IsNullOrEmpty(token)) return;
        var propertyParts = token.Split(';');
        var equipTypeValue = propertyParts[0];
        var levelValue = propertyParts.Length > 1 ? propertyParts[1] : "0";
        if (!Enum.TryParse(equipTypeValue, out EquipType equipType))
        {
            Debug.LogError($"Invalid EquipType for EquippableProperty: {equipTypeValue}");
            return;
        }
        var property = new EquippableProperty
        {
            EquipType = equipType,
            Level = int.TryParse(levelValue, out int level) ? level : 0
        };
        if (itemSettings.CurrentType != null)
        {
            itemSettings.AllProperties.Add(new ActionTypeToProperty()
            {
                ActionType = itemSettings.CurrentType,
                Property = property
            });
        }
        else
        {
            Debug.LogWarning($"No type set for EquippableProperty. Default Level: {property.Level}. Item: {itemSettings.Name}");
        }
    }
}
```

## Example Sheet Structure

For the importer to work correctly, your Google Sheet should have the following structure:

![image](https://github.com/user-attachments/assets/9aacd5a4-a6f0-48d2-be53-b42970755c65)
![image](https://github.com/user-attachments/assets/0e4dd6ee-46fb-42df-9af4-c2f2fbe58e1d)


- **Item Name**: The name of the item.
- **Is Stackable**: The stack of the item.
- **Stack Size**: The size of the stack.
- **Type**: Specifies the property type action (e.g., Equippable, Consumable).
- **EquipType**: Only used for equippable items, specifies the type of equipment, level requirements.
- **StatType**: The stat affected by the item.
- **ItemType**: The type of the item, if no equippable property was found.
---

## Contributing

Feel free to open issues or submit pull requests for new features or bug fixes. Contributions are welcome!
