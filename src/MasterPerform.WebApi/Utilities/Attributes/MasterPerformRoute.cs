using Microsoft.AspNetCore.Mvc;

namespace MasterPerform.WebApi.Utilities.Attributes
{
    /// <summary>
    /// Default route attribute.
    /// </summary>
    public class MasterPerformRoute : RouteAttribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="template">Route template.</param>
        public MasterPerformRoute(string template) :
            base($"api/master-perform/{template}")
        {
        }
    }
}
