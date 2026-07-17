using System;
using System.IO;

namespace RevitDocumentationAutomation.Infrastructure
{
    public sealed class FileLogger
    {
        public void Error(Exception exception)
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RevitDocumentationAutomation", "Logs");
            Directory.CreateDirectory(directory);
            File.AppendAllText(Path.Combine(directory, "addin.log"), DateTimeOffset.Now.ToString("O") + Environment.NewLine + exception + Environment.NewLine);
        }
    }
}
