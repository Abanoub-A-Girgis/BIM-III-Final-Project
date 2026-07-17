using System.Linq;
using Autodesk.Revit.DB;

namespace RevitDocumentationAutomation.Infrastructure
{
    public sealed class RevitParameterResolver
    {
        public SchedulableField Resolve(Autodesk.Revit.DB.ScheduleDefinition definition, BuiltInParameter parameter)
        {
            ElementId id = new ElementId(parameter);
            return definition.GetSchedulableFields().FirstOrDefault(x => x.ParameterId == id);
        }
    }
}
