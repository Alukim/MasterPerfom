using MasterPerform.Infrastructure.WebApi;

namespace MasterPerform
{
    public class Program
    {
        public static void Main(string[] args)
            => WebHostExtensions.BuildAndRunWebHost<Startup>();
    }
}
