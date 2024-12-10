using System.Collections.Generic;

namespace GoggleImporter.Runtime
{
    public interface IGoogleSheetParser
    {
        public void ParseSheet(List<string> header, IList<object> token);
    }
}