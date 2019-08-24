namespace MasterPerform.Infrastructure.Elasticsearch
{
    public class ElasticsearchSettings
    {
        public string NodeUrl { get; set; }

        public int ShardsNumber { get; set; } = 1;

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
