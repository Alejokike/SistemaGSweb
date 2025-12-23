using SistemaGS.DTO.Query;

namespace SistemaGS.DTO
{
    public class DataTablesRequest
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Search search { get; set; } = new Search();
        public ItemQuery query { get; set; } = new ItemQuery();
        public List<Column> columns { get; set; } = new List<Column>();
        public List<Order> order { get; set; } = new List<Order>();
    }

    public class Search
    {
        public string value { get; set; } = null!;
        public bool regex { get; set; } = false;
    }
    public class Column
    {
        public string data { get; set; } = null!;
        public string name { get; set; } = null!;
        public bool searchable { get; set; } = true;
        public bool orderable { get; set; } = true;
        public Search search { get; set; } = new Search();
    }
    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; } = "asc";
    }
    public class DataTablesResponse<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<T> data { get; set; } = new List<T>();
    }
}
