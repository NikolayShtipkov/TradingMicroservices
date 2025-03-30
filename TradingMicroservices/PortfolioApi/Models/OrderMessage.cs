namespace PortfolioApi.Models
{
    public class OrderMessage
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Ticker { get; set; }
        public int Quantity { get; set; }
        public string? Side { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
