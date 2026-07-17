# Developer guide

Requirements: Visual Studio 2022/.NET Framework 4.8 targeting pack, Revit 2021 API DLLs, and PowerShell. Set `RevitApiPath` at build time or install Revit at its default location. `Private=false` prevents Autodesk DLL deployment. Pure tests run independently with `dotnet test RevitDocumentationAutomation.Tests`. Add schedule fields in `DefaultScheduleDefinitions`; parameters are resolved against each category's actual schedulable fields.
