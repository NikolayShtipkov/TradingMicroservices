namespace OrderApiV2.Models
{
    public class OrderDto
    {
        public string Ticker { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Side { get; set; } = string.Empty; // "buy" or "sell"
        public decimal Price { get; set; }
    }
}
