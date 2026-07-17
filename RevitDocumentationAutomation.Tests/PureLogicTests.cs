using System;
using System.IO;
using RevitDocumentationAutomation.Infrastructure;
using Xunit;

namespace RevitDocumentationAutomation.Tests
{
    public sealed class PureLogicTests
    {
        [Fact] public void GeneratesProfessionalScheduleName() => Assert.Equal("AUTO | Door Schedule", ValidationLogic.AutomationScheduleName(" Door "));
        [Fact] public void DefinitionValidationRequiresNameAndFields() => Assert.Equal(2, ValidationLogic.ValidateDefinition("", Array.Empty<string>()).Count);
        [Fact] public void SanitizesExportFilename() => Assert.DoesNotContain(Path.GetInvalidFileNameChars()[0], ExportFileNamePolicy.Sanitize("A" + Path.GetInvalidFileNameChars()[0] + "B"));
        [Fact] public void ExcelBooleanMappingAcceptsExplicitValues() => Assert.True(ParseWorkbookBoolean("Yes"));
        [Fact] public void DetectsDuplicateDefinitionsCaseInsensitively() => Assert.Single(ValidationLogic.DuplicateNames(new[] { "Door", "door" }));
        [Theory] [InlineData(true, false, "Error")] [InlineData(false, true, "Warning")] [InlineData(false, false, "Information")] public void MapsAuditSeverity(bool blocks, bool review, string expected) => Assert.Equal(expected, ValidationLogic.SeverityFor(blocks, review));
        [Fact] public void CreatesVersionedNameOnCollision() { string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")); Directory.CreateDirectory(dir); try { File.WriteAllText(Path.Combine(dir, "Door.csv"), ""); Assert.EndsWith("Door (2).csv", ExportFileNamePolicy.NextAvailable(dir, "Door", ".csv")); } finally { Directory.Delete(dir, true); } }
        private static bool ParseWorkbookBoolean(string value) => value.Equals("yes", StringComparison.OrdinalIgnoreCase) || value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1";
    }
}
