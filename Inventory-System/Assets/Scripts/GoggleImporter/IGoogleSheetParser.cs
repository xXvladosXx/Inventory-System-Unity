using System.Collections.Generic;

namespace GoggleImporter
{
    public interface IGoogleSheetParser
    {
        public void ParseSheet(List<string> header, IList<object> token);
    }
}