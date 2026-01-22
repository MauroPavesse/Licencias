namespace Licencias.Application.Shared
{
    public class SearchCommand
    {
        public int? Id { get; set; }
        public List<SearchFilterCommand> Filters { get; set; } = [];
        public List<string> Includes { get; set; } = [];
        public bool DisableTracking { get; set; } = true;
        public int Deleted { get; set; } = 0;
    }

    public class SearchFilterCommand
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<int> Ids { get; set; } = [];
    }
}
