using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RevitDocumentationAutomation.Infrastructure
{
    public static class ValidationLogic
    {
        public static string AutomationScheduleName(string label) => "AUTO | " + (label ?? string.Empty).Trim() + " Schedule";
        public static IReadOnlyList<string> ValidateDefinition(string name, IEnumerable<string> parameters)
        {
            var errors = new List<string>(); if (string.IsNullOrWhiteSpace(name)) errors.Add("Schedule name is required."); if (parameters == null || !parameters.Any()) errors.Add("At least one field is required."); return errors;
        }
        public static IReadOnlyList<string> DuplicateNames(IEnumerable<string> names) => names.GroupBy(x => x, StringComparer.OrdinalIgnoreCase).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
        public static string SeverityFor(bool blocksIssue, bool needsReview) => blocksIssue ? "Error" : needsReview ? "Warning" : "Information";
    }
}
