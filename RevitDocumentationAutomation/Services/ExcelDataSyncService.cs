using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using ClosedXML.Excel;

namespace RevitDocumentationAutomation.Services
{
    public sealed class ExcelDataSyncService
    {
        public void Export(Document document, IEnumerable<ViewSchedule> schedules, string path)
        {
            using (var workbook = new XLWorkbook())
            {
                foreach (ViewSchedule schedule in schedules)
                {
                    string sheetName = SafeSheetName(schedule.Name, workbook.Worksheets.Select(x => x.Name));
                    IXLWorksheet sheet = workbook.Worksheets.Add(sheetName); TableData table = schedule.GetTableData(); TableSectionData body = table.GetSectionData(SectionType.Body);
                    for (int row = body.FirstRowNumber; row <= body.LastRowNumber; row++) for (int col = body.FirstColumnNumber; col <= body.LastColumnNumber; col++)
                    {
                        string value = schedule.GetCellText(SectionType.Body, row, col);
                        if (double.TryParse(value, out double number)) sheet.Cell(row - body.FirstRowNumber + 1, col - body.FirstColumnNumber + 1).Value = number; else sheet.Cell(row - body.FirstRowNumber + 1, col - body.FirstColumnNumber + 1).Value = value;
                    }
                    sheet.Columns().AdjustToContents();
                }
                workbook.SaveAs(path);
            }
        }
        private static string SafeSheetName(string name, IEnumerable<string> existing)
        {
            string clean = new string(name.Where(c => "[]:*?/\\".IndexOf(c) < 0).ToArray()); if (clean.Length > 31) clean = clean.Substring(0, 31); if (string.IsNullOrWhiteSpace(clean)) clean = "Schedule";
            string candidate = clean; for (int i = 2; existing.Contains(candidate); i++) candidate = clean.Substring(0, System.Math.Min(clean.Length, 27)) + " " + i; return candidate;
        }
    }
}
