using Microsoft.Extensions.Options;

namespace MasterPerform.Infrastructure.EnvironmentPrefixer
{
    public interface IEnvironmentPrefixer
    {
        string AppendPrefix(string data);
    }

    internal class EnvironmentPrefixer : IEnvironmentPrefixer
    {
        private readonly EnvironmentSettings settings;

        private string Format = "{0}-{1}";

        public EnvironmentPrefixer(IOptions<EnvironmentSettings> settings)
        {
            this.settings = settings.Value;
        }

        public string AppendPrefix(string data)
        {
            return string.IsNullOrWhiteSpace(settings.Prefix) ? data : string.Format(Format, settings.Prefix, data);
        }
    }
}
