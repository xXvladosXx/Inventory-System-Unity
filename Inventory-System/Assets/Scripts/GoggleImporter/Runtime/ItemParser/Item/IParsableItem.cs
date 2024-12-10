using System.Collections.Generic;
using GoggleImporter.Runtime.ItemParser.Types;

namespace GoggleImporter.Runtime.ItemParser.Item
{
    public interface IParsableItem
    {
        Dictionary<ActionType, List<Property.Property>> Properties { get; }
    }
}