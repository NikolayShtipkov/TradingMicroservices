namespace OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Side { get; set; } = string.Empty; // "buy" or "sell"
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
