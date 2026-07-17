# Architecture

The ribbon `App` launches four thin external commands. Commands validate Revit context and cancellation, services execute schedule creation/export, Excel mapping, and audit workflows, models carry typed definitions/results, UI owns instance view models, and infrastructure isolates parameter resolution, collision naming, and logging. Schedule batches use a `TransactionGroup`; each definition uses an accurately named transaction. Unsupported optional fields become warnings, required fields fail that schedule, and the group is rolled back when no changes succeed.

Automation owns only schedules prefixed `AUTO |`. A same-name manual schedule is skipped, preventing destructive updates. Audit checks are independent methods so rules can be added without changing command orchestration.
