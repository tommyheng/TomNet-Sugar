using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.SqlSugarCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        public SqlSugarClient DbContext { get; }
        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _dbFactory = serviceProvider.GetDbFactory();
            DbContext = _dbFactory.GetDbContext();
        }
        public void BeginTran()
        {
            DbContext.Ado.BeginTran();
        }

        public void Commit()
        {
            DbContext.Ado.CommitTran();
        }

        public void Rollback()
        {
            DbContext.Ado.RollbackTran();
        }

    }
}
