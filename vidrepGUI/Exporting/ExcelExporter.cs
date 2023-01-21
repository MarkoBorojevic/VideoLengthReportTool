using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.WindowsAPICodePack.Shell;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace vidrepGUI
{
    public class ExcelExporter : DataExporter
    {
        public override string FriendlyName => "Excel Spreadsheet";

        public override bool DecideOutputPath(out string path)
        {
            var dlg = new SaveFileDialog();

            dlg.Filter = "Excel File (*.xls)|*.xls";
            dlg.RestoreDirectory = true;

            path = "";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path = dlg.FileName;
                return true;
            }

            return false;
        }

        public override void Export(FileEntry[] files, string xlFileOut)
        {
            Application xlApp;
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Valid?";
            xlWorkSheet.Cells[1, 2] = "File Name";
            xlWorkSheet.Cells[1, 3] = "Duration";
            xlWorkSheet.Cells[1, 4] = "Date Created";
            xlWorkSheet.Cells[1, 5] = "Full Path";

            for (int i = 0; i < files.Length; i++)
            {
                xlWorkSheet.Cells[i + 2, 1] = files[i].valid;
                xlWorkSheet.Cells[i + 2, 2] = Path.GetFileName(files[i].filePath);
                xlWorkSheet.Cells[i + 2, 3] = !files[i].valid ? "N/A" : files[i].VideoLength;
                xlWorkSheet.Cells[i + 2, 4] = !files[i].valid ? "N/A" : files[i].DateCreated;
                xlWorkSheet.Cells[i + 2, 5] = files[i].filePath;

                xlWorkSheet.Range[$"A{i + 2}", $"F{i + 2}"].Font.Color = ColorTranslator.ToOle(files[i].valid ? Color.Black : Color.Red);
            }

            xlWorkBook.SaveAs(xlFileOut, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
