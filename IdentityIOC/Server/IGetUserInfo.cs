using IdentityIOC.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityIOC.Server
{
   public interface IGetUserInfo
    {
        public Task<UserInfo> GetName();
        public void OnGet(string Message);
    }
}
