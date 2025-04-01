using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueStory.Domain.Interfaces;

namespace TrueStory.Domain.Aggregate.Product
{
    /// <summary>
    /// Interface for Product Repository
    /// </summary>
    public interface IProductRepository : IBaseRepository<T_Product,Guid>
    {
    }
}
