# Portfolio case study

## Problem and solution

Architectural teams repeatedly configure schedules and inspect documentation health. A typed configuration and category-aware parameter resolver reduce repetition while avoiding assumptions about field availability. Commands separate UI orchestration from transaction services, return accurate Revit results, and log unexpected failures. ClosedXML provides validated Excel-to-Revit configuration and Revit schedule-to-workbook data flow without COM. Pure policies are unit tested; host behavior has a repeatable manual plan. The result demonstrates Revit API commands, schedule automation, validation, exchange, QA/QC, and maintainable handover—without claiming client deployment or invented savings.

## How I Would Extend This for Full Drawing Production Automation

These are proposed, not implemented: Excel-driven sheet and view creation; configurable view scale/naming; view and schedule placement; title-block selection; door, finishes, and ironmongery schedules; parameter-driven detail swapping; pyRevit deployment; Dynamo custom Python nodes; and Rhino.Inside.Revit integration. A next phase would model these as validated definitions, preflight collisions, then execute atomic Revit transaction groups with placement diagnostics.
