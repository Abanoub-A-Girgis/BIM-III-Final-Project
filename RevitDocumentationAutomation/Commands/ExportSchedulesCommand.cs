using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitDocumentationAutomation.Configuration;
using RevitDocumentationAutomation.Infrastructure;
using RevitDocumentationAutomation.Services;
using RevitDocumentationAutomation.UI;

namespace RevitDocumentationAutomation.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public sealed class ExportSchedulesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveUIDocument?.Document; if (document == null) { message = "Open a project document first."; return Result.Failed; }
                var schedules = new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().Where(x => !x.IsTemplate && !x.IsTitleblockRevisionSchedule).OrderBy(x => x.Name).ToList(); if (!schedules.Any()) { message = "This project contains no exportable schedules."; return Result.Failed; }
                var model = new ScheduleSelectionViewModel(); foreach (ViewSchedule schedule in schedules) model.Items.Add(new ScheduleSelectionViewModel.Item { Value = schedule, Label = schedule.Name, IsSelected = schedule.Name.StartsWith(DefaultScheduleDefinitions.Prefix, StringComparison.Ordinal) });
                if (new ScheduleSelectionWindow(model, "Export Schedules", "Choose folder").ShowDialog() != true) return Result.Cancelled;
                var selected = model.Selected.Cast<ViewSchedule>().ToList(); if (!selected.Any()) return Result.Cancelled;
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog { Description = "Choose the CSV output directory" }) { if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled; var failures = new ScheduleExportService().Export(selected, dialog.SelectedPath); TaskDialog.Show("Schedule Export", "Exported: " + (selected.Count - failures.Count) + "\nFailed: " + failures.Count + (failures.Any() ? "\n\n" + string.Join("\n", failures) : string.Empty)); if (failures.Count == selected.Count) { message = string.Join(Environment.NewLine, failures); return Result.Failed; } }
                return Result.Succeeded;
            }
            catch (Exception ex) { new FileLogger().Error(ex); message = "Schedule export failed. Verify the directory is writable; details were logged."; return Result.Failed; }
        }
    }
}
