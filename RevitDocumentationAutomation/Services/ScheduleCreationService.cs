using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using RevitDocumentationAutomation.Configuration;
using RevitDocumentationAutomation.Infrastructure;
using RevitDocumentationAutomation.Models;
using ModelScheduleDefinition = RevitDocumentationAutomation.Models.ScheduleDefinition;

namespace RevitDocumentationAutomation.Services
{
    public sealed class ScheduleCreationService
    {
        private readonly RevitParameterResolver resolver;
        public ScheduleCreationService(RevitParameterResolver resolver) { this.resolver = resolver; }

        public ScheduleCreationResult Apply(Document document, IEnumerable<ModelScheduleDefinition> definitions)
        {
            var result = new ScheduleCreationResult();
            using (var group = new TransactionGroup(document, "Create or Update Documentation Schedules"))
            {
                group.Start();
                foreach (ModelScheduleDefinition requested in definitions)
                {
                    try { ApplyOne(document, requested, result); }
                    catch (Exception ex) { result.Failures.Add(requested.Name + ": " + ex.Message); }
                }
                if (result.Created.Count + result.Updated.Count > 0) group.Assimilate(); else group.RollBack();
            }
            return result;
        }

        private void ApplyOne(Document document, ModelScheduleDefinition requested, ScheduleCreationResult result)
        {
            ViewSchedule schedule = new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().FirstOrDefault(x => !x.IsTemplate && x.Name.Equals(requested.Name, StringComparison.OrdinalIgnoreCase));
            bool create = schedule == null;
            if (!create && !schedule.Name.StartsWith(DefaultScheduleDefinitions.Prefix, StringComparison.Ordinal)) { result.Skipped.Add(requested.Name); return; }
            using (var transaction = new Transaction(document, (create ? "Create " : "Update ") + requested.Name.Replace(DefaultScheduleDefinitions.Prefix, string.Empty)))
            {
                transaction.Start();
                if (create) { schedule = ViewSchedule.CreateSchedule(document, new ElementId(requested.Category)); schedule.Name = requested.Name; }
                Autodesk.Revit.DB.ScheduleDefinition target = schedule.Definition;
                while (target.GetFieldCount() > 0) target.RemoveField(target.GetFieldId(0));
                target.ClearSortGroupFields();
                target.IsItemized = requested.ItemizeEveryInstance;
                ScheduleField sortField = null;
                foreach (ScheduleFieldDefinition field in requested.Fields)
                {
                    SchedulableField available = resolver.Resolve(target, field.Parameter);
                    if (available == null)
                    {
                        string warning = requested.Name + ": parameter " + field.Parameter + " is unavailable.";
                        if (field.Required) throw new InvalidOperationException(warning);
                        result.Warnings.Add(warning); continue;
                    }
                    ScheduleField added = target.AddField(available);
                    if (!string.IsNullOrWhiteSpace(field.Heading)) added.ColumnHeading = field.Heading;
                    if (requested.SortParameter == field.Parameter) sortField = added;
                }
                if (sortField != null) target.AddSortGroupField(new ScheduleSortGroupField(sortField.FieldId) { ShowHeader = requested.ShowGroupHeader });
                else if (requested.SortParameter.HasValue) result.Warnings.Add(requested.Name + ": sort parameter is unavailable; grouping was omitted.");
                transaction.Commit();
            }
            if (create) result.Created.Add(requested.Name); else result.Updated.Add(requested.Name);
        }
    }
}
