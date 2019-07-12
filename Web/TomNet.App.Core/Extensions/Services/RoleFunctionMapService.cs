using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomNet.App.Model.DB.Identity;
using TomNet.App.Model.DB.Security;
using TomNet.App.Model.DTO;
using TomNet.Caching;
using TomNet.Dependency;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.App.Core.Services.Security
{
    public partial class RoleFunctionMapService
    {
        public void SyncToCache()
        {
            var db = _repository.UnitOfWork.DbContext;
            var cache = _serviceProvider.GetService<IDistributedCache>();
            var lst = db.Queryable<RoleFunctionMap, Role, Function>((rolefunction, role, func)
                   => rolefunction.RoleId == role.Id && rolefunction.FunctionId == func.Id)
                   .Where((rolefunction, role, func) => func.AccessType != FunctionAccessType.Anonymous && func.IsController == false)
                   .Select((rolefunction, role, func) => new RoleFunctionMapDto
                   {
                       RoleId = rolefunction.RoleId,
                       //RoleName = role.Name,
                       Source = func.Source,
                       IsWebApi = func.IsWebApi,
                       Area = func.Area,
                       Controller = func.Controller,
                       Action = func.Action,
                       IsLocked = func.IsLocked
                   }).ToArray();
            var roles = db.Queryable<Role>().ToArray();

            for (int i = 0; i < roles.Length; i++)
            {
                var k = "_role_" + roles[i].Id.ToString();
                var v = lst.Where(m => m.RoleId == roles[i].Id).ToArray();

                cache.Remove(k);
                cache.Set(k, v);
            }
        }


        public void SyncToCache(int roleId)
        {
            var db = _repository.UnitOfWork.DbContext;
            var cache = _serviceProvider.GetService<IDistributedCache>();
            var lst = db.Queryable<RoleFunctionMap, Role, Function>((rolefunction, role, func)
                   => rolefunction.RoleId == role.Id && rolefunction.FunctionId == func.Id)
                   .Where((rolefunction, role, func) => role.Id == roleId
                   && func.AccessType != FunctionAccessType.Anonymous
                   && func.IsController == false)
                   .Select((rolefunction, role, func) => new RoleFunctionMapDto
                   {
                       RoleId = rolefunction.RoleId,
                       //RoleName = role.Name,
                       Source = func.Source,
                       IsWebApi = func.IsWebApi,
                       Area = func.Area,
                       Controller = func.Controller,
                       Action = func.Action,
                       IsLocked = func.IsLocked
                   }).ToArray();

            var k = "_role_" + roleId;
            var v = lst.ToArray();

            cache.Remove(k);
            cache.Set(k, v);

        }

        public bool Authorize(RoleFunctionMapDto dto)
        {
            var cache = _serviceProvider.GetService<IDistributedCache>();
            var k = "_role_" + dto.RoleId.ToString();
            var roles = cache.Get<RoleFunctionMapDto[]>(k);

            return roles.Any(m =>
                m.Source == dto.Source
                && m.RoleId == dto.RoleId
                && m.Area == dto.Area
                && m.Controller == dto.Controller
                && m.Action == dto.Action
                && m.IsLocked == false);
        }
    }
}
