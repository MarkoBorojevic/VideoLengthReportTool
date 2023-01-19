using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.WindowsAPICodePack.Shell;

namespace VideoLengthReportTool
{
    public class LengthTool
    {
        public static async Task<string> ExportVideoDurations(string[] files, string xlFileOut)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Application xlApp;
                    Workbook xlWorkBook;
                    Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;

                    xlApp = new Application();
                    xlWorkBook = xlApp.Workbooks.Add(misValue);

                    xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    xlWorkSheet.Cells[1, 1] = "File Name";
                    xlWorkSheet.Cells[1, 2] = "Duration";

                    for (int i = 0; i < files.Length; i++)
                    {
                        xlWorkSheet.Cells[i + 2, 1] = files[i];
                        xlWorkSheet.Cells[i + 2, 2] = GetVideoDuration(files[i]).ToString(@"mm\:ss");
                    }

                    xlWorkBook.SaveAs(xlFileOut, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);

                    return "success";
                } catch(Exception e)
                {
                    return e.Message;
                }
            });
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

        private static TimeSpan GetVideoDuration(string filePath)
        {
            using (var shell = ShellObject.FromParsingName(filePath))
            {
                var prop = shell.Properties.System.Media.Duration;
                var t = (ulong)prop.ValueAsObject;
                return TimeSpan.FromTicks((long)t);
            }
        }
    }
}
