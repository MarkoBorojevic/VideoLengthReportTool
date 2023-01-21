using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vidrepGUI
{
    public partial class Exporter : Form
    {
        DataExporter[] exporters = new DataExporter[]
        {
            new ExcelExporter()
        };

        DataExporter exporter => (DataExporter)exportTypeBox.SelectedValue;

        FileEntry[] data;

        public Exporter(FileEntry[] data)
        {
            this.data = data;
            InitializeComponent();
        }

        private void Export_Load(object sender, EventArgs e)
        {
            exportTypeBox.DataSource = exporters;
            sortBox.DataSource = Enum.GetValues(typeof(ExportParameters.SortType));
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            ExportParameters exParams = new ExportParameters()
            {
                sortBy = (ExportParameters.SortType)sortBox.SelectedValue
            };

            switch(exParams.sortBy)
            {
                case ExportParameters.SortType.FileName:
                    Array.Sort(data, (x, y) => String.Compare(x.filePath, y.filePath));
                    break;
                case ExportParameters.SortType.DateCreated:
                    data = data.OrderBy(d => !d.valid ? DateTime.MinValue : DateTime.ParseExact(d.DateCreated, "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture)).ToArray();
                    break;
            }

            if (exporter.DecideOutputPath(out string path))
            {
                Export(path);
            }
        }

        void Export(string path)
        {
            exportButton.Enabled = false;
            exportButton.Text = "Exporting...";

            try
            {
                exporter.Export(data, path);
                MessageBox.Show($"Exported to {path}", "Export finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception er)
            {
                if (MessageBox.Show($"Error during export:\n\n{er}", "Failed to export", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    Export(path);
            }
            finally
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
