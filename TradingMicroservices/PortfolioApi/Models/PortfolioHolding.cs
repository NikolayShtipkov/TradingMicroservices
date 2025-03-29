namespace PortfolioApi.Models
{
    public class PortfolioHolding
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public string? Ticker { get; set; }
        public int Quantity { get; set; }

        public Portfolio? Portfolio { get; set; }
    }
}
