namespace Lapka.Pets.Infrastructure.Options
{
    public class ElasticSearchOptions
    {
        public string Url { get; set; }
        public ElasticAliases Aliases { get; set; }
    }
}