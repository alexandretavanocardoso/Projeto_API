using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;

namespace DevIO.Api.Swagger
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public static Dictionary<string, string> Resources { get; set; } = new Dictionary<string, string>();

        public void Apply(ControllerModel controller)
        {
            Resources.Add(controller.ControllerType.AssemblyQualifiedName, string.IsNullOrEmpty(controller.ApiExplorer.GroupName) ? controller.ControllerName : controller.ApiExplorer.GroupName);
        }
    }
}