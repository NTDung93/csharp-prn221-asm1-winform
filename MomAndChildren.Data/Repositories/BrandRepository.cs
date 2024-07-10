using MomAndChildren.Data.Base;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Data.Repositories
{
    public class BrandRepository : GenericRepository<Brand>
    {
        public BrandRepository() { }
        public BrandRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;
    }
}
