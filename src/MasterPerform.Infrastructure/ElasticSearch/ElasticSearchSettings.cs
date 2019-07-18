namespace MasterPerform.Infrastructure.Elasticsearch
{
    public class ElasticsearchSettings
    {
        public string NodeUrl { get; set; }

        public int ShardsNumber { get; set; } = 1;
    }
}
