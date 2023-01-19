using System.Collections.Specialized;
using System.IO;

namespace VideoLengthReportTool.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            videoFilesBox.SelectedIndexChanged += (s, a) => UpdateUI();
            videoFilesBox.SelectionMode = SelectionMode.MultiExtended;

            UpdateUI();
        }

        void UpdateUI()
        {
            removeFileButton.Enabled = videoFilesBox.SelectedIndex != -1;
        }

        static string[] mediaExtensions = {
            "*.wav",".aac",".wma",".wmv",".avi",".mpg",".mpeg",".m1v",".mp2",".mp3",".mpa",".mpe",".m3u",".mp4",".mov",".3g2",".3gp2",".3gp",".3gpp",".m4a",".cda",".aif",".aifc",".aiff",".mid",".midi",".rmi",".mkv",".WAV",".AAC",".WMA",".WMV",".AVI",".MPG",".MPEG",".M1V",".MP2",".MP3",".MPA",".MPE",".M3U",".MP4",".MOV",".3G2",".3GP2",".3GP",".3GPP",".M4A",".CDA",".AIF",".AIFC",".AIFF",".MID",".MIDI",".RMI",".MKV"
        };

        private void addItemButton_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                videoFilesBox.Items.AddRange(dlg.FileNames);
            }
        }

        private void removeFileButton_Click(object sender, EventArgs e)
        {
            foreach(int i in videoFilesBox.SelectedIndices)
                videoFilesBox.Items.RemoveAt(i);
        }

        private async void exportButton_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();

            dlg.Filter = "Excel File (*.xls)|*.xls";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                exportButton.Enabled = false;
                exportButton.Text = "Exporting...";

                MessageBox.Show($"Export status: {await LengthTool.ExportVideoDurations(videoFilesBox.Items.Cast<string>().ToArray(), dlg.FileName)}", $"{dlg.FileName}");

                exportButton.Text = "Export";
                exportButton.Enabled = true;
            }
        }

        private void videoFilesBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                foreach (var str in e.Data.GetData(DataFormats.FileDrop) as string[])
                {
                    if (Directory.Exists(str))
                    {
                        videoFilesBox.Items.AddRange(CheckDirectory(str));
                    }
                    else if (mediaExtensions.Contains(Path.GetExtension(str)))
                    {
                        videoFilesBox.Items.Add(str);
                    }
                }
            } catch(Exception er)
            {
                MessageBox.Show($"Failed to import\n\n{er.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string[] CheckDirectory(string directory)
        {
            List<string> files = new List<string>();

            foreach(var path in Directory.GetFiles(directory))
            {
                if(mediaExtensions.Contains(Path.GetExtension(path)))
                {
                    files.Add(path);
                }
            }

            foreach(var subfolder in Directory.GetDirectories(directory))
            {
                files.AddRange(CheckDirectory(subfolder));
            }

            return files.ToArray();
        }

        private void videoFilesBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}