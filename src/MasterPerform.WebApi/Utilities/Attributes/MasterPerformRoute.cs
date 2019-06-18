using Microsoft.AspNetCore.Mvc;

namespace MasterPerform.WebApi.Utilities.Attributes
{
    public class MasterPerformRoute : RouteAttribute
    {
        public MasterPerformRoute(string template) :
            base($"api/master-perform/{template}")
        {
        }
    }
}
