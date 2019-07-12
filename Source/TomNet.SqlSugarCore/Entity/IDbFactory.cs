using System;

using SqlSugar;


namespace TomNet.SqlSugarCore.Entity
{
    public interface IDbFactory
    {
        SqlSugarClient GetDbContext();
        SqlSugarClient GetDbContext(string dbContextKey);
        SqlSugarClient GetDbContext(Action<Exception> onErrorEvent);
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> OnLogExecutingEvent);
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> OnLogExecutingEvent, Action<Exception> onErrorEvent);
        SqlSugarClient GetDbContext(string dbContextKey, Action<string, SugarParameter[]> OnLogExecutingEvent, Action<Exception> onErrorEvent);
    }
}
