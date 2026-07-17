# Revit Documentation Automation Toolkit

A Revit 2021 add-in by Abanoub Girgis that reduces repetitive schedule setup and applies consistent documentation standards across supported Revit categories.

## Business problem and features

Manual schedule setup is repetitive and inconsistent. The toolkit provides idempotent schedule creation for framing, columns, walls, doors, windows, floors, and roofs; validated Excel-driven configuration; selected CSV export; Excel schedule-data export services; and a documentation QA/QC audit.

## Screenshots

> Screenshot placeholder: ribbon, schedule selection, Excel validation, and audit results.

## Architecture

Commands coordinate Revit and dialogs; services contain workflows; models carry results/configuration; infrastructure resolves Revit parameters, filenames, and logs. See [Documentation/ARCHITECTURE.md](Documentation/ARCHITECTURE.md).

## Compatibility and build

Targets Revit 2021 and .NET Framework 4.8. Install Revit 2021 or provide its API directory:

```powershell
.\build.ps1 -RevitApiPath 'C:\Program Files\Autodesk\Revit 2021'
```

Autodesk DLLs are never copied or packaged. ClosedXML 0.95.4 is used because it supports .NET Framework 4.8 without Excel COM automation.

## Installation and usage

Run `package.ps1`, then extract the packaged files directly into `%AppData%\Autodesk\Revit\Addins\2021`. Launch Revit, open a project, and use the **Documentation Automation** ribbon buttons. The Excel workbook uses `Schedules` columns `Enabled, ScheduleName, Category, ItemizeEveryInstance, SortParameter, ShowGroupHeader` and `Fields` columns `ScheduleName, Order, ParameterIdentifier, DisplayHeading, Required`.

## QA/QC rules and limitations

Implemented checks cover missing door/window marks, duplicate sheet numbers, empty sheets, unplaced printable views, and schedule naming. Tag-presence inference is deliberately not claimed: Revit 2021 cannot reliably infer expected tags across every plan and annotation convention. Existing `AUTO |` schedules are automation-managed; manually named schedules are not overwritten.

## Roadmap

Shared-parameter GUIDs, configurable naming regexes, reliable tag rules for constrained templates, sheet/view production, and multi-version builds.

## License

MIT. See [LICENSE](LICENSE).
