using System.ComponentModel;
using TomNet.AspNetCore.Data;
using TomNet.AspNetCore.Mvc;
using TomNet.Data;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.App.WebApi.Controllers
{
    [Description("API-Default")]
    public class DefaultController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        IRepository<Function, int> _repository;
        public DefaultController(IUnitOfWork unitOfWork,
            IRepository<Function, int> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        [Description("Index")]
        public AjaxResult Index()
        {
            return new AjaxResult("允许访问", AjaxResultType.Success);
        }
    }
}