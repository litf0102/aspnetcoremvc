using aspnetcoremvc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcoremvc.Repositories
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
    }
}
