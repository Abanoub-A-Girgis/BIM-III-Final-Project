using System;
using System.Collections.Generic;
using System.IO;
using Autodesk.Revit.DB;
using RevitDocumentationAutomation.Infrastructure;

namespace RevitDocumentationAutomation.Services
{
    public sealed class ScheduleExportService
    {
        public IList<string> Export(IEnumerable<ViewSchedule> schedules, string directory)
        {
            if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("The selected export directory does not exist.");
            string probe = Path.Combine(directory, ".write-test-" + Guid.NewGuid().ToString("N"));
            File.WriteAllText(probe, string.Empty); File.Delete(probe);
            var failures = new List<string>();
            foreach (ViewSchedule schedule in schedules)
            {
                try
                {
                    string target = ExportFileNamePolicy.NextAvailable(directory, schedule.Name, ".csv");
                    schedule.Export(directory, Path.GetFileName(target), new ViewScheduleExportOptions());
                }
                catch (Exception ex) { failures.Add(schedule.Name + ": " + ex.Message); }
            }
            return failures;
        }
    }
}
