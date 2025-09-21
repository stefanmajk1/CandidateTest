using CandidateTest.API.DTO;
using CandidateTest.Models.Entities;

namespace CandidateTest.API.ProductMapper
{
    public interface IProductMapper
    {
        ProductDto ToDto(ProductEntity e);
        List<ProductDto> ToDtoList(IEnumerable<ProductEntity> items);
        ProductEntity FromCreateDto(ProductCreateDto dto);
        void UpdateEntity(ProductEntity entity, ProductUpdateDto dto);
    }

    public class ProductMapper : IProductMapper
    {
        public ProductDto ToDto(ProductEntity e) => new ProductDto
        {
            ProductId = e.ProductId,
            ProductName = e.ProductName,
            Price = e.Price,
            Description = e.Description,
            StockQuantity = e.StockQuantity
        };

        public List<ProductDto> ToDtoList(IEnumerable<ProductEntity> items)
            => items.Select(ToDto).ToList();

        public ProductEntity FromCreateDto(ProductCreateDto dto) => new ProductEntity
        {
            ProductName = dto.ProductName,
            Price = dto.Price,
            Description = dto.Description,
            StockQuantity = dto.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        public void UpdateEntity(ProductEntity entity, ProductUpdateDto dto)
        {
            entity.ProductName = dto.ProductName;
            entity.Price = dto.Price;
            entity.Description = dto.Description;
            entity.StockQuantity = dto.StockQuantity;
            // entity.UpdatedAt = DateTime.UtcNow; // ako uvedeš kolonu
        }
    }
}
