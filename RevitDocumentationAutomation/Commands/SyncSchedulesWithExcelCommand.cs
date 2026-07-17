using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using RevitDocumentationAutomation.Infrastructure;
using RevitDocumentationAutomation.Services;

namespace RevitDocumentationAutomation.Commands
{
    [Transaction(TransactionMode.Manual)]
    public sealed class SyncSchedulesWithExcelCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveUIDocument?.Document; if (document == null || document.IsFamilyDocument) { message = "Open a Revit project document first."; return Result.Failed; }
                var choice = new TaskDialog("Excel Schedule Exchange") { MainInstruction = "Choose an Excel operation", CommonButtons = TaskDialogCommonButtons.Cancel };
                choice.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Import schedule configuration", "Validate workbook rows, then create or update schedules.");
                choice.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Export schedule contents", "Write all automation schedules to worksheets.");
                TaskDialogResult choiceResult = choice.Show(); if (choiceResult == TaskDialogResult.Cancel) return Result.Cancelled;
                if (choiceResult == TaskDialogResult.CommandLink2)
                {
                    var save = new SaveFileDialog { Filter = "Excel workbook (*.xlsx)|*.xlsx", FileName = "RevitScheduleData.xlsx", Title = "Export schedule data" }; if (save.ShowDialog() != true) return Result.Cancelled;
                    var schedules = new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().Where(x => !x.IsTemplate && x.Name.StartsWith("AUTO | ", StringComparison.Ordinal)).ToList();
                    if (!schedules.Any()) { message = "No automation schedules are available to export."; return Result.Failed; }
                    new ExcelDataSyncService().Export(document, schedules, save.FileName); TaskDialog.Show("Excel Export", "Exported " + schedules.Count + " schedules."); return Result.Succeeded;
                }
                var open = new OpenFileDialog { Filter = "Excel workbook (*.xlsx)|*.xlsx", Title = "Choose schedule configuration workbook" }; if (open.ShowDialog() != true) return Result.Cancelled;
                var definitions = new ScheduleConfigurationService().Read(open.FileName, out var errors); if (errors.Any()) { message = "Workbook validation failed:\n" + string.Join("\n", errors.Take(20)); TaskDialog.Show("Excel Validation", message); return Result.Failed; }
                var result = new ScheduleCreationService(new RevitParameterResolver()).Apply(document, definitions); TaskDialog.Show("Excel Sync", result.ToSummary()); if (result.Failures.Any()) { message = string.Join(Environment.NewLine, result.Failures); return Result.Failed; } return Result.Succeeded;
            }
            catch (Exception ex) { new FileLogger().Error(ex); message = "Excel synchronization failed. Details were logged."; return Result.Failed; }
        }
    }
}
