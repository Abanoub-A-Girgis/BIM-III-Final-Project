using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using ClosedXML.Excel;
using RevitDocumentationAutomation.Models;
using ModelScheduleDefinition = RevitDocumentationAutomation.Models.ScheduleDefinition;

namespace RevitDocumentationAutomation.Services
{
    public sealed class ScheduleConfigurationService
    {
        public IReadOnlyList<ModelScheduleDefinition> Read(string path, out IReadOnlyList<string> errors)
        {
            var failures = new List<string>(); var definitions = new List<ModelScheduleDefinition>();
            using (var workbook = new XLWorkbook(path))
            {
                IXLWorksheet schedules = workbook.Worksheet("Schedules"); IXLWorksheet fields = workbook.Worksheet("Fields");
                var fieldRows = fields.RowsUsed().Skip(1).GroupBy(x => x.Cell(1).GetString(), StringComparer.OrdinalIgnoreCase).ToDictionary(x => x.Key, x => x, StringComparer.OrdinalIgnoreCase);
                foreach (IXLRow row in schedules.RowsUsed().Skip(1))
                {
                    if (!ParseBool(row.Cell(1).GetString())) continue;
                    string name = row.Cell(2).GetString(); string categoryText = row.Cell(3).GetString();
                    if (!Enum.TryParse(categoryText, true, out BuiltInCategory category)) { failures.Add("Unknown category '" + categoryText + "' for " + name + ". Use an OST_* BuiltInCategory name."); continue; }
                    var definition = new ModelScheduleDefinition { Name = name, Category = category, ItemizeEveryInstance = ParseBool(row.Cell(4).GetString()), ShowGroupHeader = ParseBool(row.Cell(6).GetString()) };
                    if (Enum.TryParse(row.Cell(5).GetString(), true, out BuiltInParameter sort)) definition.SortParameter = sort;
                    if (!fieldRows.TryGetValue(name, out var configuredFields)) { failures.Add("No Fields rows found for " + name + "."); continue; }
                    foreach (IXLRow field in configuredFields.OrderBy(x => x.Cell(2).GetValue<int>()))
                    {
                        string parameterText = field.Cell(3).GetString();
                        if (!Enum.TryParse(parameterText, true, out BuiltInParameter parameter)) { failures.Add("Unknown parameter '" + parameterText + "' for " + name + "."); continue; }
                        definition.Fields.Add(new ScheduleFieldDefinition(parameter, field.Cell(4).GetString(), ParseBool(field.Cell(5).GetString())));
                    }
                    definitions.Add(definition);
                }
            }
            foreach (var duplicate in definitions.GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase).Where(x => x.Count() > 1)) failures.Add("Duplicate schedule definition: " + duplicate.Key);
            errors = failures; return definitions;
        }
        private static bool ParseBool(string value) => value.Equals("true", StringComparison.OrdinalIgnoreCase) || value.Equals("yes", StringComparison.OrdinalIgnoreCase) || value == "1";
    }
}
