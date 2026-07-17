using System.Collections.Generic;
using Autodesk.Revit.DB;
using RevitDocumentationAutomation.Models;
using ModelScheduleDefinition = RevitDocumentationAutomation.Models.ScheduleDefinition;

namespace RevitDocumentationAutomation.Configuration
{
    public static class DefaultScheduleDefinitions
    {
        public const string Prefix = "AUTO | ";
        public static IReadOnlyList<ModelScheduleDefinition> Create() => new[]
        {
            Def("Structural Framing Schedule", BuiltInCategory.OST_StructuralFraming, BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, BuiltInParameter.ELEM_FAMILY_PARAM, BuiltInParameter.ELEM_TYPE_PARAM, BuiltInParameter.INSTANCE_LENGTH_PARAM),
            Def("Structural Column Schedule", BuiltInCategory.OST_StructuralColumns, BuiltInParameter.SCHEDULE_BASE_LEVEL_PARAM, BuiltInParameter.SCHEDULE_TOP_LEVEL_PARAM, BuiltInParameter.ELEM_TYPE_PARAM),
            Def("Wall Schedule", BuiltInCategory.OST_Walls, BuiltInParameter.WALL_BASE_CONSTRAINT, BuiltInParameter.ELEM_TYPE_PARAM, BuiltInParameter.INSTANCE_LENGTH_PARAM, BuiltInParameter.HOST_AREA_COMPUTED),
            Def("Door Schedule", BuiltInCategory.OST_Doors, BuiltInParameter.ALL_MODEL_MARK, BuiltInParameter.ELEM_FAMILY_PARAM, BuiltInParameter.ELEM_TYPE_PARAM),
            Def("Window Schedule", BuiltInCategory.OST_Windows, BuiltInParameter.ALL_MODEL_MARK, BuiltInParameter.ELEM_FAMILY_PARAM, BuiltInParameter.ELEM_TYPE_PARAM),
            Def("Floor Schedule", BuiltInCategory.OST_Floors, BuiltInParameter.SCHEDULE_LEVEL_PARAM, BuiltInParameter.ELEM_TYPE_PARAM, BuiltInParameter.HOST_AREA_COMPUTED),
            Def("Roof Schedule", BuiltInCategory.OST_Roofs, BuiltInParameter.SCHEDULE_LEVEL_PARAM, BuiltInParameter.ELEM_TYPE_PARAM, BuiltInParameter.ROOF_SLOPE, BuiltInParameter.HOST_AREA_COMPUTED)
        };
        private static ModelScheduleDefinition Def(string suffix, BuiltInCategory category, params BuiltInParameter[] fields)
        {
            var definition = new ModelScheduleDefinition { Name = Prefix + suffix, Category = category, SortParameter = BuiltInParameter.ELEM_TYPE_PARAM, ShowGroupHeader = true };
            foreach (BuiltInParameter field in fields) definition.Fields.Add(new ScheduleFieldDefinition(field, field.ToString(), field == BuiltInParameter.ELEM_TYPE_PARAM));
            definition.Fields.Add(new ScheduleFieldDefinition(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS, "Comments"));
            return definition;
        }
    }
}
