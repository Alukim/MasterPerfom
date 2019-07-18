using MasterPerform.Infrastructure.WebApi;

namespace MasterPerform.WebApi
{
    /// <summary>
    /// Main class of program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that will start program.
        /// </summary>
        /// <param name="args">Arguments to starting application.</param>
        public static void Main(string[] args)
            => WebHostExtensions.BuildAndRunWebHost<Startup>();
    }
}
