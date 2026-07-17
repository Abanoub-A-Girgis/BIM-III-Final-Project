using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitDocumentationAutomation.Configuration;
using RevitDocumentationAutomation.Infrastructure;
using RevitDocumentationAutomation.Models;
using RevitDocumentationAutomation.Services;
using RevitDocumentationAutomation.UI;
using ModelScheduleDefinition = RevitDocumentationAutomation.Models.ScheduleDefinition;

namespace RevitDocumentationAutomation.Commands
{
    [Transaction(TransactionMode.Manual)]
    public sealed class CreateSchedulesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveUIDocument?.Document; if (document == null) { message = "Open a project document first."; return Result.Failed; } if (document.IsFamilyDocument) { message = "Schedules cannot be created in a family document."; return Result.Failed; }
                var existing = new FilteredElementCollector(document).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>().Select(x => x.Name).ToList(); var model = new ScheduleSelectionViewModel();
                foreach (ModelScheduleDefinition definition in DefaultScheduleDefinitions.Create()) model.Items.Add(new ScheduleSelectionViewModel.Item { Value = definition, Label = definition.Name, IsSelected = true, Exists = existing.Contains(definition.Name) });
                if (new ScheduleSelectionWindow(model, "Create or Update Schedules", "Create / update").ShowDialog() != true) return Result.Cancelled;
                var selected = model.Selected.Cast<ModelScheduleDefinition>().ToList(); if (selected.Count == 0) return Result.Cancelled;
                ScheduleCreationResult result = new ScheduleCreationService(new RevitParameterResolver()).Apply(document, selected); TaskDialog.Show("Documentation Automation", result.ToSummary() + (result.Warnings.Any() ? "\n\n" + string.Join("\n", result.Warnings.Take(8)) : string.Empty));
                if (result.Failures.Any()) { message = string.Join(Environment.NewLine, result.Failures); return Result.Failed; } return Result.Succeeded;
            }
            catch (Exception ex) { new FileLogger().Error(ex); message = "Schedule automation failed. Details were written to the local add-in log."; return Result.Failed; }
        }
    }
}
