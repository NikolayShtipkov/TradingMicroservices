namespace PriceApi.Models
{
    public class Price
    {
        public int Id { get; set; }
        public string? Ticker { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
