namespace CandidateTest.WinForms.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
    }

    // DTO za kreiranje (POST)  
    public class ProductCreateDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
    }

    // DTO za izmenu (PUT)
    public class ProductUpdateDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
    }
}
