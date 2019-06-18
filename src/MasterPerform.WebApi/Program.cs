using MasterPerform.Infrastructure.WebApi;

namespace MasterPerform.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
            => WebHostExtensions.BuildAndRunWebHost<Startup>();
    }
}
