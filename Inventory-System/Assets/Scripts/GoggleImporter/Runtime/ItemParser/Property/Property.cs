using System;

namespace GoggleImporter.Runtime.ItemParser.Property
{
    [Serializable]
    public abstract class Property
    {
        public bool ResetableOnImport = true;
    }
}