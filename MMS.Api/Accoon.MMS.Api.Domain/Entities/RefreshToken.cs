using Accoon.MMS.Api.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Domain.Entities
{
    public class RefreshToken : Entity<Guid>
    {
        public string Token { get;  set; }
        public DateTime Expires { get;  set; }
        public string UserId { get;  set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get;  set; }

        public RefreshToken()
        {

        }

        public RefreshToken(string token, DateTime expires, string userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
