using System.Collections.Generic;

namespace RevitDocumentationAutomation.Models
{
    public enum AuditSeverity { Information, Warning, Error }
    public sealed class AuditIssue
    {
        public AuditSeverity Severity { get; set; }
        public string RuleName { get; set; }
        public int ElementId { get; set; }
        public string Category { get; set; }
        public string ElementName { get; set; }
        public string Description { get; set; }
        public string RecommendedAction { get; set; }
    }
    public sealed class AuditReport { public IList<AuditIssue> Issues { get; } = new List<AuditIssue>(); }
}
