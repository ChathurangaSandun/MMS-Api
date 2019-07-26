using Accoon.MMS.Api.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accoon.MMS.Api.Domain.Entities
{
    public class User : Entity<Guid>
    {
        public string FirstName { get;  set; } // EF migrations require at least private setter - won't work on auto-property
        public string LastName { get; set; }
        public string IdentityId { get; set; }
        public string UserName { get; set; } // Required by automapper
        public string Email { get; set; }
        public string PasswordHash { get; private set; }

        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public User() { /* Required by EF */ }

        internal User(string firstName, string lastName, string identityId, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentityId = identityId;
            UserName = userName;
        }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, string userId, string remoteIpAddress, double daysToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), userId, remoteIpAddress));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }
    }
}
