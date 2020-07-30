using AutoMapper;
using aspnetcoremvc.Entities;
using aspnetcoremvc.Services.UserApp.Dtos;
using aspnetcoremvc.Services.RoleApp.Dtos;
using aspnetcoremvc.Services.DepartmentApp.Dtos;
using aspnetcoremvc.Services.MenuApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcoremvc.Utils
{
    /// <summary>
    /// Enity与Dto映射
    /// </summary>
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Menu, MenuDto>();
            CreateMap<MenuDto, Menu>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<RoleDto, Role>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleMenuDto, RoleMenu>();
            CreateMap<RoleMenu, RoleMenuDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserRoleDto, UserRole>();
            CreateMap<UserRole, UserRoleDto>();
        }
    }
}
