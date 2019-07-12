using System;
using System.Collections.Generic;
using System.Text;
using TomNet.App.Model.DTO;

namespace TomNet.App.Core.Contracts.Security
{
    public partial interface IRoleFunctionMapContract
    {
        void SyncToCache();
        void SyncToCache(int roleId);
        bool Authorize(RoleFunctionMapDto dto);
    }
}
