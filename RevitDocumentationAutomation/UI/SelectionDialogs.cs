using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RevitDocumentationAutomation.Models;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Text;

namespace RevitDocumentationAutomation.UI
{
    public sealed class ScheduleSelectionViewModel
    {
        public sealed class Item { public object Value { get; set; } public string Label { get; set; } public bool IsSelected { get; set; } public bool Exists { get; set; } }
        public IList<Item> Items { get; } = new List<Item>();
        public IEnumerable<object> Selected => Items.Where(x => x.IsSelected).Select(x => x.Value);
    }

    public sealed class ScheduleSelectionWindow : Window
    {
        private readonly ScheduleSelectionViewModel model;
        public ScheduleSelectionWindow(ScheduleSelectionViewModel model, string title, string action)
        {
            this.model = model; Title = title; Width = 500; Height = 480; WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var root = new DockPanel { Margin = new Thickness(12) }; var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            AddButton(buttons, "Select all", (s,e) => SetAll(true)); AddButton(buttons, "Clear all", (s,e) => SetAll(false)); AddButton(buttons, action, (s,e) => { DialogResult = true; Close(); }); AddButton(buttons, "Cancel", (s,e) => { DialogResult = false; Close(); });
            DockPanel.SetDock(buttons, Dock.Bottom); root.Children.Add(buttons);
            var list = new ListBox();
            foreach (var item in model.Items) { var check = new CheckBox { Content = item.Label + (item.Exists ? "  (exists)" : string.Empty), IsChecked = item.IsSelected, Margin = new Thickness(4), Tag = item }; check.Checked += (s,e) => ((ScheduleSelectionViewModel.Item)((CheckBox)s).Tag).IsSelected = true; check.Unchecked += (s,e) => ((ScheduleSelectionViewModel.Item)((CheckBox)s).Tag).IsSelected = false; list.Items.Add(check); }
            root.Children.Add(list); Content = root;
        }
        private void SetAll(bool value) { foreach (var item in model.Items) item.IsSelected = value; foreach (CheckBox check in ((ListBox)((DockPanel)Content).Children[1]).Items) check.IsChecked = value; }
        private static void AddButton(Panel panel, string label, RoutedEventHandler handler) { var button = new Button { Content = label, Margin = new Thickness(4), Padding = new Thickness(10,5,10,5) }; button.Click += handler; panel.Children.Add(button); }
    }

    public sealed class AuditResultsWindow : Window
    {
        public AuditResultsWindow(AuditReport report, UIDocument uiDocument)
        {
            Title = "Documentation Model Audit"; Width = 1000; Height = 550; WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var root = new DockPanel(); var grid = new DataGrid { ItemsSource = report.Issues, IsReadOnly = true, AutoGenerateColumns = true, CanUserSortColumns = true };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            AddAuditButton(buttons, "Select in Revit", (s,e) => { if (grid.SelectedItem is AuditIssue issue && issue.ElementId > 0) { uiDocument.Selection.SetElementIds(new[] { new ElementId(issue.ElementId) }); uiDocument.ShowElements(new ElementId(issue.ElementId)); } });
            AddAuditButton(buttons, "Export CSV", (s,e) => { var dialog = new Microsoft.Win32.SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = "DocumentationAudit.csv" }; if (dialog.ShowDialog() == true) File.WriteAllText(dialog.FileName, ToCsv(report)); });
            DockPanel.SetDock(buttons, Dock.Bottom); root.Children.Add(buttons); root.Children.Add(grid); Content = root;
        }
        private static void AddAuditButton(Panel panel, string text, RoutedEventHandler action) { var button = new Button { Content = text, Margin = new Thickness(5), Padding = new Thickness(10,5,10,5) }; button.Click += action; panel.Children.Add(button); }
        private static string ToCsv(AuditReport report) { var text = new StringBuilder("Severity,Rule,ElementId,Category,Element,Description,RecommendedAction\r\n"); foreach (AuditIssue issue in report.Issues) text.AppendLine(string.Join(",", Csv(issue.Severity.ToString()), Csv(issue.RuleName), issue.ElementId, Csv(issue.Category), Csv(issue.ElementName), Csv(issue.Description), Csv(issue.RecommendedAction))); return text.ToString(); }
        private static string Csv(string value) => "\"" + (value ?? string.Empty).Replace("\"", "\"\"") + "\"";
    }
}
