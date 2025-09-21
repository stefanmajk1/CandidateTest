using CandidateTest.API.DTO;
using CandidateTest.API.ProductMapper;
using CandidateTest.API.Repositories;
using CandidateTest.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CandidateTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IProductMapper _mapper;

        public ProductsController(IUnitOfWork uow, IProductMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: /api/products?name=...&minPrice=...&maxPrice=...&categories=1,3&page=1&pageSize=20
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? categories,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            IEnumerable<int>? categoryIds = null;
            if (!string.IsNullOrWhiteSpace(categories))
            {
                categoryIds = categories
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .Distinct()
                    .ToList();
            }

            var (items, total) = await _uow.Products.GetAllFilteredPagedAsync(
                name, minPrice, maxPrice, categoryIds, page, pageSize);

            Response.Headers["X-Total-Count"] = total.ToString();

            return Ok(_mapper.ToDtoList(items));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var entity = await _uow.Products.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.ToDto(entity));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
        {
            var entity = _mapper.FromCreateDto(dto);
            await _uow.Products.CreateAsync(entity);
            await _uow.CommitAsync();

            var createdDto = _mapper.ToDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.ProductId }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (id != dto.ProductId) return BadRequest();

            var entity = await _uow.Products.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _mapper.UpdateEntity(entity, dto);
            await _uow.Products.UpdateAsync(entity);
            await _uow.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _uow.Products.DeleteAsync(id);
            await _uow.CommitAsync();
            return NoContent();
        }
    }
}
