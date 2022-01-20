using EventFool.Domain;
using EventFool.Domain.Models;
using EventFool.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(Eventfool dbcontext) : base(dbcontext)
        {

        }

        public override void Update(Category category)
        {
            _dbContext.Entry(category).State = System.Data.Entity.EntityState.Modified;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _dbContext.Categories;
        }

        public Category Read(string categoryName)
        {
            return _dbContext.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
        }

        
    }
}
