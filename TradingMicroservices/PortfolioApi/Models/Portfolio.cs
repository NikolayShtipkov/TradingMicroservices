namespace PortfolioApi.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal TotalValue { get; set; }

        public List<PortfolioHolding> Holdings { get; set; } = new();
    }
}
