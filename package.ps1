param([string]$Configuration = 'Release', [string]$RevitApiPath = "$env:ProgramW6432\Autodesk\Revit 2021")
$ErrorActionPreference = 'Stop'
& "$PSScriptRoot\build.ps1" -Configuration $Configuration -RevitApiPath $RevitApiPath
$release = Join-Path $PSScriptRoot 'artifacts\RevitDocumentationAutomation'
New-Item -ItemType Directory -Force -Path $release | Out-Null
Copy-Item "$PSScriptRoot\RevitDocumentationAutomation\bin\$Configuration\net48\*" $release -Recurse -Force -Exclude RevitAPI.dll,RevitAPIUI.dll
Copy-Item "$PSScriptRoot\README.md" $release
Compress-Archive -Path "$release\*" -DestinationPath "$PSScriptRoot\artifacts\RevitDocumentationAutomation.zip" -Force
