using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitDocumentationAutomation.Infrastructure;
using RevitDocumentationAutomation.Services;
using RevitDocumentationAutomation.UI;

namespace RevitDocumentationAutomation.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public sealed class RunModelAuditCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try { UIDocument uiDocument = commandData.Application.ActiveUIDocument; Document document = uiDocument?.Document; if (document == null || document.IsFamilyDocument) { message = "Open a Revit project document first."; return Result.Failed; } new AuditResultsWindow(new ModelAuditService().Run(document), uiDocument).ShowDialog(); return Result.Succeeded; }
            catch (Exception ex) { new FileLogger().Error(ex); message = "The model audit failed. Details were logged."; return Result.Failed; }
        }
    }
}
