namespace MasterPerform.Infrastructure.ElasticSearch
{
    public class ElasticSearchSettings
    {
        public string NodeUrl { get; set; }

        public int ShardsNumber { get; set; } = 1;
    }
}
