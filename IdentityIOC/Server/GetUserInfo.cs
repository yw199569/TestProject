using IdentityIOC.ServiceModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityIOC.Server
{
    public class GetUserInfo: IGetUserInfo
    {
       private readonly ILogger<GetUserInfo> _logger;

        public GetUserInfo(ILogger<GetUserInfo> logger)
        {
            _logger = logger;
        }

        public async Task<UserInfo> GetName()
        {
            UserInfo info = new UserInfo {Name="tets" };
            return info;

        }
        public string Message { get; set; }

        public void OnGet(string Message)
        {
            _logger.LogInformation(Message);
        }

    }
}
