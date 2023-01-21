using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vidrepGUI
{
    public class FileEntry
    {
        public bool valid { get; private set; }
        public string filePath { get; private set; }

        public string VideoLength { get; private set; }
        public string DateCreated { get; private set; }

        public override string ToString() => $"{filePath}";

        public FileEntry(string filePath)
        {
            this.filePath = filePath;

            try
            {
                using (var shell = ShellObject.FromParsingName(filePath))
                {
                    valid = shell != null;

                    if (valid)
                    {
                        VideoLength = TimeSpan.FromTicks((long)shell.Properties.System.Media.Duration.Value).ToString(@"hh\:mm\:ss");
                        DateCreated = shell.Properties.System.DateCreated.Value.Value.ToString("dd/MM/yyyy");
                    }
                }
            }
            catch
            {
                valid = false;
            }
        }
    }
}
