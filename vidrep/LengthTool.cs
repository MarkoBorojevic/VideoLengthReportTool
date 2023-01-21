using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.WindowsAPICodePack.Shell;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace vidrep
{
    public class LengthTool
    {
        public static void ExportVideoDurations(FileEntry[] files, string xlFileOut)
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

            for (int i = 0; i < files.Length; i++)
            {
                xlWorkSheet.Cells[i + 2, 1] = files[i].valid;
                xlWorkSheet.Cells[i + 2, 2] = Path.GetFileName(files[i].filePath);
                xlWorkSheet.Cells[i + 2, 3] = !files[i].valid ? "N/A" : files[i].VideoLength;
                xlWorkSheet.Cells[i + 2, 4] = !files[i].valid ? "N/A" : files[i].DateCreated;

                xlWorkSheet.Range[$"A{i + 2}", $"F{i + 2}"].Font.Color = ColorTranslator.ToOle(files[i].valid ? Color.Black : Color.Red);
            }

            xlWorkBook.SaveAs(xlFileOut, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        private static void releaseObject(object obj)
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
