namespace AuthAPI.Controllers
{
    public class ProductReqDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Units { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
    }
}
