using aspnetcoremvc.Entities;
using aspnetcoremvc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcoremvc.Repositories
{
    public class MenuRepository : MVCCoreRepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(MVCCOREDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
