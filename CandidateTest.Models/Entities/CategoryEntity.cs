using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateTest.Models.Entities
{
    public  class CategoryEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public ICollection<ProductCategoryEntity> ProductCategories { get; set; } = new List<ProductCategoryEntity>();
    }
}
