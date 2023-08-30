namespace ResellHub.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual List<Offer> Offers { get; set; }
    }
}