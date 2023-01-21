using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vidrepGUI
{
    public static class Extensions 
    { 
        public static string TruncateForDisplay(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var returnValue = value;
            if (value.Length > length)
            {
                var tmp = value.Substring(0, length);
                if (tmp.LastIndexOf(' ') > 0)
                    returnValue = tmp.Substring(0, tmp.LastIndexOf(' ')) + " ...";
            }
            return returnValue;
        }
    }
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            videoFilesBox.SelectedIndexChanged += (s, a) => UpdateUI();
            videoFilesBox.SelectionMode = SelectionMode.MultiExtended;

            UpdateUI();
        }

        void UpdateUI()
        {
            removeFileButton.Enabled = videoFilesBox.SelectedIndex != -1;
            openButton.Enabled = videoFilesBox.SelectedIndex != -1;
        }

        static string[] mediaExtensions = {
            ".wav",".aac",".wma",".wmv",".avi",".mpg",".mpeg",".m1v",".mp2",".mp3",".mpa",".mpe",".m3u",".mp4",".mov",".3g2",".3gp2",".3gp",".3gpp",".m4a",".cda",".aif",".aifc",".aiff",".mid",".midi",".rmi",".mkv",".WAV",".AAC",".WMA",".WMV",".AVI",".MPG",".MPEG",".M1V",".MP2",".MP3",".MPA",".MPE",".M3U",".MP4",".MOV",".3G2",".3GP2",".3GP",".3GPP",".M4A",".CDA",".AIF",".AIFC",".AIFF",".MID",".MIDI",".RMI",".MKV"
        };

        string searchPattern => $"*{string.Join(";*", mediaExtensions)}";

        void AddFileEntry(params FileEntry[] entries)
        {
            BeginInvoke((Action)(() =>
            {
                List<FileEntry> cleanedArray = entries.ToList();
                FileEntry[] currentEntries = videoFilesBox.Items.Cast<FileEntry>().ToArray();

                foreach (var entry in entries)
                {
                    if (currentEntries.Where(x => x.filePath.Equals(entry.filePath)).ToArray().Length > 0)
                    {
                        cleanedArray.Remove(entry);
                    }
                }
                //if (entries.Select(x => !x.valid) != null)
                //    MessageBox.Show("Some imported videos may be corrupted.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                videoFilesBox.Items.AddRange(cleanedArray.ToArray());
            }));
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = $"All Media Files|{searchPattern}";
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddFileEntry(dlg.FileNames.Select(x => new FileEntry(x)).ToArray());
            }
        }

        private void removeFileButton_Click(object sender, EventArgs e)
        {
            while(videoFilesBox.SelectedIndices.Count > 0)
            {
                foreach (int index in videoFilesBox.SelectedIndices)
                    videoFilesBox.Items.RemoveAt(index);
            }
        }

        private async void exportButton_Click(object sender, EventArgs e)
        {
            new Exporter(videoFilesBox.Items.Cast<FileEntry>().ToArray()).ShowDialog(this);
        }

        private void videoFilesBox_DragDrop(object sender, DragEventArgs e)
        {
            var dialog = new WaitDialog("Importing", "Importing, please wait...", async (ct) =>
            {
                foreach (var str in e.Data.GetData(DataFormats.FileDrop) as string[])
                {
                    if (ct.IsCancellationRequested)
                        break;
                    if (Directory.Exists(str))
                    {
                        AddFileEntry(await RecursiveDirectorySearch(str, ct));
                    }
                    else if (mediaExtensions.Contains(Path.GetExtension(str)))
                    {
                        AddFileEntry(new FileEntry(str));
                    }
                }
            });
            if(dialog.ShowDialog() == DialogResult.OK)
            {
               // MessageBox.Show("imported");
            }
        }

        async Task<FileEntry[]> RecursiveDirectorySearch(string directory, CancellationToken ct)
        {
            List<FileEntry> files = new List<FileEntry>();

            try
            {
                foreach (var path in Directory.GetFiles(directory))
                {
                    if (ct.IsCancellationRequested) break;
                    if (mediaExtensions.Contains(Path.GetExtension(path)))
                    {
                        if (ct.IsCancellationRequested) break;
                        try
                        {
                            WaitDialog.instance.UpdateText($"Importing, please wait...\n{Path.GetDirectoryName(path).TruncateForDisplay(20)}\\{Path.GetFileName(path)}");
                        }
                        catch { break; }
                        files.Add(new FileEntry(path));
                    }
                }

                foreach (var subfolder in Directory.GetDirectories(directory))
                {
                    if (ct.IsCancellationRequested) break;
                    files.AddRange(await RecursiveDirectorySearch(subfolder, ct));
                }
            } catch { }

            return files.ToArray();
        }

        private void videoFilesBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            //Process fileopener = new Process();

            //fileopener.StartInfo.FileName = "explorer";
            //fileopener.StartInfo.Arguments = "\"" + ((FileEntry)videoFilesBox.SelectedItem).filePath + "\"";
            //fileopener.Start();

            player.URL = ((FileEntry)videoFilesBox.SelectedItem).filePath;
        }
    }
}