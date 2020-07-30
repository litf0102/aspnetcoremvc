using aspnetcoremvc.Entities;
using aspnetcoremvc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcoremvc.Repositories
{
    public class DepartmentRepository : MVCCoreRepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MVCCOREDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
