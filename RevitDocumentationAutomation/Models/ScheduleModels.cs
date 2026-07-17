using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RevitDocumentationAutomation.Models
{
    public sealed class ScheduleFieldDefinition
    {
        public ScheduleFieldDefinition(BuiltInParameter parameter, string heading, bool required = false) { Parameter = parameter; Heading = heading; Required = required; }
        public BuiltInParameter Parameter { get; }
        public string Heading { get; }
        public bool Required { get; }
    }

    public sealed class ScheduleDefinition
    {
        public string Name { get; set; }
        public BuiltInCategory Category { get; set; }
        public IList<ScheduleFieldDefinition> Fields { get; set; } = new List<ScheduleFieldDefinition>();
        public BuiltInParameter? SortParameter { get; set; }
        public bool ShowGroupHeader { get; set; }
        public bool ItemizeEveryInstance { get; set; } = true;
    }

    public sealed class ScheduleCreationResult
    {
        public IList<string> Created { get; } = new List<string>();
        public IList<string> Updated { get; } = new List<string>();
        public IList<string> Skipped { get; } = new List<string>();
        public IList<string> Warnings { get; } = new List<string>();
        public IList<string> Failures { get; } = new List<string>();
        public string ToSummary() => $"Created: {Created.Count}\nUpdated: {Updated.Count}\nSkipped: {Skipped.Count}\nWarnings: {Warnings.Count}\nFailures: {Failures.Count}";
    }
}
