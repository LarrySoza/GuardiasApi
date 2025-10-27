namespace App.WebApi.Entities
{
    public class PaginaDatosModel<T>
    {
        public int total { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public List<T> data { get; set; } = new List<T>();
    }
}
