param([string]$Configuration = 'Release', [string]$RevitApiPath = "$env:ProgramW6432\Autodesk\Revit 2021")
$ErrorActionPreference = 'Stop'
dotnet restore .\RevitDocumentationAutomation.sln
dotnet build .\RevitDocumentationAutomation.sln -c $Configuration -p:RevitApiPath="$RevitApiPath" --no-restore
dotnet test .\RevitDocumentationAutomation.Tests\RevitDocumentationAutomation.Tests.csproj -c $Configuration --no-build
