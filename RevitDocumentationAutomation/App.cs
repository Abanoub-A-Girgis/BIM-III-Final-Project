using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace RevitDocumentationAutomation
{
    /// <summary>Creates the Documentation Automation ribbon.</summary>
    public sealed class App : IExternalApplication
    {
        private const string TabName = "Documentation Automation";
        public Result OnStartup(UIControlledApplication application)
        {
            try { application.CreateRibbonTab(TabName); } catch (Autodesk.Revit.Exceptions.ArgumentException) { }
            RibbonPanel panel = application.CreateRibbonPanel(TabName, "Documentation Automation");
            string assembly = Assembly.GetExecutingAssembly().Location;
            AddButton(panel, assembly, "CreateSchedules", "Create\nSchedules", "RevitDocumentationAutomation.Commands.CreateSchedulesCommand", "Create or safely update configured schedules.", "Beam.png");
            AddButton(panel, assembly, "ExcelSync", "Excel\nSync", "RevitDocumentationAutomation.Commands.SyncSchedulesWithExcelCommand", "Validate Excel configuration, create schedules, or export schedule data.", "Column.png");
            AddButton(panel, assembly, "ExportSchedules", "Export\nSchedules", "RevitDocumentationAutomation.Commands.ExportSchedulesCommand", "Choose actual Revit schedules and export them to CSV.", "Export.png");
            AddButton(panel, assembly, "ModelAudit", "Model\nAudit", "RevitDocumentationAutomation.Commands.RunModelAuditCommand", "Run documentation-focused QA/QC checks and review findings.", "Wall.png");
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application) { return Result.Succeeded; }

        private static void AddButton(RibbonPanel panel, string assembly, string id, string text, string className, string tooltip, string icon)
        {
            var button = (PushButton)panel.AddItem(new PushButtonData(id, text, assembly, className));
            button.ToolTip = tooltip;
            button.LongDescription = tooltip + " Operations are validated and unexpected failures are logged.";
            try { button.LargeImage = new BitmapImage(new Uri("pack://application:,,,/RevitDocumentationAutomation;component/Icons/" + icon)); } catch (Exception) { }
        }
    }
}
