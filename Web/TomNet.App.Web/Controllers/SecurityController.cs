using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using TomNet.App.Core.Contracts.Identity;
using TomNet.App.Core.Contracts.Security;
using TomNet.App.Model.DB.Identity;
using TomNet.App.Model.DB.Security;
using TomNet.App.Model.DTO;
using TomNet.AspNetCore.Data;
using TomNet.Collections;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.App.Web.Controllers
{
    [Authorize]
    [Description("系统安全")]
    public class SecurityController : Controller
    {
        private readonly IRoleContract _roleContract;
        private readonly IRoleFunctionMapContract _roleFunctionMapContract;
        private readonly IUnitOfWork _unitOfWork;
        public SecurityController(IRoleContract roleContract,
            IUnitOfWork unitOfWork,
            IRoleFunctionMapContract roleFunctionMapContract)
        {
            _roleContract = roleContract;
            _unitOfWork = unitOfWork;
            _roleFunctionMapContract = roleFunctionMapContract;
        }

        [Description("权限列表")]
        [HttpGet]
        public IActionResult RoleFunctionMapList()
        {
            return View();
        }

        [Description("权限列表")]
        [HttpPost]
        public IActionResult RoleFunctionMapList(int roleId = 0, string src = "")
        {
            var query = _unitOfWork.DbContext.Queryable<Function, RoleFunctionMap>((func, roleFunc)
                => new object[] { JoinType.Left, func.Id == roleFunc.FunctionId })
                .Where((func, roleFunc) => func.AccessType != SqlSugarCore.Entity.FunctionAccessType.Anonymous && func.IsController == false)
                .WhereIF(!string.IsNullOrEmpty(src), func => func.Source == src)
                .OrderBy((func, roleFunc) => func.Source, OrderByType.Asc)
                .Select((func, roleFunc) => new RoleFunctionViewDto
                {
                    id = func.Id,
                    name = func.Name,
                    area = func.Area,
                    controller = func.Controller,
                    action = func.Action,
                    accesstype = func.AccessType,
                    islocked = func.IsLocked,
                    iswebapi = func.IsWebApi,
                    iscontroller = func.IsController,
                    source = func.Source,
                    LAY_CHECKED = SqlFunc.Subqueryable<RoleFunctionMap>().Where(m => m.FunctionId == func.Id && m.RoleId == roleId).Any(),
                    //count = SqlFunc.Subqueryable<RoleFunctionMap>().Where(m => m.FunctionId == func.Id && m.RoleId == 1).Count()
                }).ToList();
            var result = new
            {
                code = 0,
                msg = "Success",
                count = 0,
                data = query.ToArray()
            };

            JsonSerializerSettings settings = new JsonSerializerSettings();
            //EF Core中默认为驼峰样式序列化处理key
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //使用默认方式，不更改元数据的key的大小写
            settings.ContractResolver = new DefaultContractResolver();
            return Json(result, settings);
        }

        [Description("设置权限")]
        [HttpPost]
        public IActionResult RoleFunctionMapSet(RoleFunctionSetDto dto)
        {
            AjaxResult result = new AjaxResult
            {
                Type = Data.AjaxResultType.Success,
                Content = "操作成功：<br>"
            };
            int delCount = 0;
            int addCount = 0;

            //特殊处理
            if (dto.FunctionIds == null || dto.FunctionIds.Length == 0)
            {
                delCount = _roleFunctionMapContract.Delete(m => m.RoleId == dto.RoleId);
            }
            else
            {
                try
                {
                    var dbItem = _roleFunctionMapContract.Entities.Where(m => m.RoleId == dto.RoleId).ToArray();

                    int count = dto.FunctionIds.Length;
                    RoleFunctionMap[] arr = new RoleFunctionMap[count];
                    for (int i = 0; i < count; i++)
                    {
                        arr[i] = new RoleFunctionMap
                        {
                            RoleId = dto.RoleId,
                            FunctionId = dto.FunctionIds[i]
                        };

                    }

                    _roleFunctionMapContract.UnitOfWork.BeginTran();


                    //删除
                    var delItem = dbItem.Except(arr, EqualityHelper<RoleFunctionMap>.CreateComparer(m => m.RoleId + "-" + m.FunctionId)).ToArray();
                    if (delItem.Length > 0)
                    {
                        delCount = _roleFunctionMapContract.Delete(delItem.Select(m => m.Id).ToArray());
                    }

                    //新增
                    var addItem = arr.Except(dbItem, EqualityHelper<RoleFunctionMap>.CreateComparer(m => m.RoleId + "-" + m.FunctionId)).ToArray();
                    if (addItem.Length > 0)
                    {
                        addCount = _roleFunctionMapContract.Insert(addItem);
                    }

                    _roleFunctionMapContract.UnitOfWork.Commit();

                }
                catch (Exception)
                {
                    _roleFunctionMapContract.UnitOfWork.Rollback();
                }
            }

            if (addCount > 0)
            {
                result.Content += "新增 " + addCount + " 条权限<br>";
            }
            if (delCount > 0)
            {
                result.Content += "删除 " + delCount + " 条权限<br>";
            }
            if (addCount == 0 && delCount == 0)
            {
                result.Content += "无任何修改！";
            }
            else
            {
                //同步到缓存
                _roleFunctionMapContract.SyncToCache(dto.RoleId);
            }

            return Json(result);
        }

        [Description("角色列表")]
        [HttpGet]
        public IActionResult RoleList()
        {
            return View();
        }

        [Description("角色列表")]
        [HttpPost]
        public IActionResult RoleList(int page = 1, int limit = 20, string name = "")
        {
            var query = _roleContract.Entities;
            query.WhereIF(!string.IsNullOrEmpty(name), m => m.Name.Contains(name));
            int total = 0;
            var list = query.ToPageList(page, limit, ref total);
            var result = new LayuiPageResult<Role>
            {
                Code = 0,
                Msg = "Success",
                Count = total,
                Data = list.ToArray()
            };
            return Json(result);
        }

        [Description("获取角色和Func的来源")]
        [HttpPost]
        public IActionResult AllRoleAndSource()
        {
            var roles = _roleContract.Entities.ToArray();
            var source = _unitOfWork.DbContext.Queryable<Function>().GroupBy(m => m.Source).Select(m => m.Source).ToArray();

            AjaxResult result = new AjaxResult
            {
                Type = Data.AjaxResultType.Success,
                Content = "操作成功",
                Data = new { roles, source }
            };

            return Json(result);
        }

        [Description("添加角色")]
        [HttpGet]
        public IActionResult RoleInsert()
        {
            return View();
        }

        [Description("添加角色")]
        [HttpPost]
        public IActionResult RoleInsert(Role role)
        {
            AjaxResult result = new AjaxResult { Type = Data.AjaxResultType.Error, Content = "未知异常", Data = null };

            var count = _roleContract.Entities.Where(m => m.Name == role.Name).Count();
            if (count > 0)
            {
                result.Content = "有相同账号";
            }
            else
            {
                result.Content = "操作成功";
                result.Type = Data.AjaxResultType.Success;
                _roleContract.Insert(role);
            }

            return Json(result);
        }


    }
}