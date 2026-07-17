using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using RevitDocumentationAutomation.Configuration;
using RevitDocumentationAutomation.Models;

namespace RevitDocumentationAutomation.Services
{
    public sealed class ModelAuditService
    {
        public AuditReport Run(Document document)
        {
            var report = new AuditReport();
            CheckMarks(document, BuiltInCategory.OST_Doors, report); CheckMarks(document, BuiltInCategory.OST_Windows, report);
            foreach (ViewSchedule schedule in new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().Where(x => !x.IsTemplate && !x.IsTitleblockRevisionSchedule && !x.Name.StartsWith(DefaultScheduleDefinitions.Prefix, StringComparison.Ordinal)))
                Add(report, AuditSeverity.Information, "Schedule naming convention", schedule, "Schedule does not use the AUTO | convention.", "Rename it only if it is automation-managed.");
            var sheets = new FilteredElementCollector(document).OfClass(typeof(ViewSheet)).Cast<ViewSheet>().ToList();
            foreach (var duplicate in sheets.GroupBy(x => x.SheetNumber).Where(x => x.Count() > 1)) foreach (ViewSheet sheet in duplicate) Add(report, AuditSeverity.Error, "Duplicate sheet number", sheet, "Sheet number is duplicated: " + duplicate.Key, "Assign a unique sheet number.");
            foreach (ViewSheet sheet in sheets.Where(x => x.GetAllPlacedViews().Count == 0)) Add(report, AuditSeverity.Warning, "Empty sheet", sheet, "Sheet contains no placed views.", "Place a view or remove the sheet if obsolete.");
            var placed = new HashSet<ElementId>(sheets.SelectMany(x => x.GetAllPlacedViews()));
            foreach (View view in new FilteredElementCollector(document).OfClass(typeof(View)).Cast<View>().Where(x => !x.IsTemplate && x.CanBePrinted && !(x is ViewSchedule) && !(x is ViewSheet) && !placed.Contains(x.Id))) Add(report, AuditSeverity.Information, "View not placed", view, "Printable view is not placed on a sheet.", "Review and place it or mark it as working content.");
            return report;
        }
        private static void CheckMarks(Document document, BuiltInCategory category, AuditReport report)
        {
            foreach (Element element in new FilteredElementCollector(document).OfCategory(category).WhereElementIsNotElementType())
                if (string.IsNullOrWhiteSpace(element.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString())) Add(report, AuditSeverity.Warning, "Missing instance mark", element, "Door or window has an empty Mark.", "Assign a unique Mark before issuing documentation.");
        }
        private static void Add(AuditReport report, AuditSeverity severity, string rule, Element element, string description, string action) => report.Issues.Add(new AuditIssue { Severity = severity, RuleName = rule, ElementId = element.Id.IntegerValue, Category = element.Category?.Name ?? "View", ElementName = element.Name, Description = description, RecommendedAction = action });
    }
}
