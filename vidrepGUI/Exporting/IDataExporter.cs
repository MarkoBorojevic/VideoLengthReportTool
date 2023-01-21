using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vidrepGUI
{
    public struct ExportParameters
    {
        public enum SortType
        {
            FileName,
            DateCreated
        }

        public SortType sortBy;
    }
    public abstract class DataExporter
    {
        public abstract string FriendlyName { get; }

        public override string ToString() => FriendlyName;

        public abstract bool DecideOutputPath(out string path);
        public abstract void Export(FileEntry[] data, string outputPath);
    }
}
