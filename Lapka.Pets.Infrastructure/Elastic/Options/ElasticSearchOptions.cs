namespace Lapka.Pets.Infrastructure.Elastic.Options
{
    public class ElasticSearchOptions
    {
        public string Url { get; set; }
        public ElasticAliases Aliases { get; set; }
    }
}