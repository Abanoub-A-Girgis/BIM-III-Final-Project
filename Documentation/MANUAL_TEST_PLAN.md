# Manual test plan

Use a disposable Revit 2021 project and retain a backup.

1. Create each of seven schedules independently; verify exact `AUTO |` names and supported columns.
2. Run creation twice; verify no duplicate and an update count on the second run.
3. Manually edit an automation schedule and confirm update behavior; create a manually named schedule and verify it is untouched.
4. Request an unavailable optional/required parameter in Excel; verify warning/failure respectively.
5. Close every dialog and folder picker; verify cancellation and no model change.
6. Export one and several schedules; verify CSVs and versioned collision names.
7. Choose an invalid/read-only directory; verify a clear failure and log entry.
8. Import the sample workbook, then corrupt category, parameter, and duplicate names; verify validation occurs before transactions.
9. Create unmarked doors/windows, duplicate sheet numbers, empty sheets, and unplaced views; verify audit findings.
10. Open a clean model and exercise all four ribbon commands.

These are manual Revit integration tests; no automated Revit-host coverage is claimed.
