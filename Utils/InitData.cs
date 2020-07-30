using aspnetcoremvc.Entities;
using aspnetcoremvc.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcoremvc.Utils
{
    public static class InitData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCCOREDbContext(serviceProvider.GetRequiredService<DbContextOptions<MVCCOREDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }
                Guid departmentId = Guid.NewGuid();
                // 部署追加
                context.Departments.Add(
                   new Department
                   {
                       Id = departmentId,
                       Name = "Group1",
                       ParentId = Guid.Empty
                   }
                );
                // スーパーユーザ追加
                context.Users.Add(
                     new User
                     {
                         UserName = "admin",
                         Password = "123456", //暂不进行加密
                         Name = "Administrator",
                         DepartmentId = departmentId
                     }
                );
                // メニュー追加
                context.Menus.AddRange(
                   new Menu
                   {
                       Name = "事業者管理",
                       Code = "Department",
                       SerialNumber = 0,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "権限管理",
                       Code = "Role",
                       SerialNumber = 2,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "ユーザ管理",
                       Code = "User",
                       SerialNumber = 1,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "グループ管理",
                       Code = "Group",
                       SerialNumber = 3,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   }
                );
                //ロール追加
                Guid roleId = Guid.NewGuid();
                context.Roles.AddRange(
                    new Role
                    {
                        Id = roleId,
                        Code = "Role1",
                        Name = "Role1"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
