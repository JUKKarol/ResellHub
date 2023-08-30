namespace ResellHub.DTOs.SharedDTOs
{
    public class PagedRespondListDto<T>
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public List<T> Items { get; set; }
    }
}