using System.IO;
using System.Linq;

namespace RevitDocumentationAutomation.Infrastructure
{
    public static class ExportFileNamePolicy
    {
        public static string Sanitize(string value)
        {
            string cleaned = new string(value.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c).ToArray()).Trim().TrimEnd('.');
            return string.IsNullOrWhiteSpace(cleaned) ? "Schedule" : cleaned;
        }
        public static string NextAvailable(string directory, string name, string extension)
        {
            string path = Path.Combine(directory, Sanitize(name) + extension);
            for (int version = 2; File.Exists(path); version++) path = Path.Combine(directory, Sanitize(name) + " (" + version + ")" + extension);
            return path;
        }
    }
}
