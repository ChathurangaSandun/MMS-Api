using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Domain.Exceptions
{
    public class AppUserCreationFaildException: Exception
    {
        public IEnumerable<IdentityError> identityErrors { get; }

        public AppUserCreationFaildException(string message, IEnumerable<IdentityError> identityErrors) : base(message)
        {
            this.identityErrors = identityErrors;
        }

        public AppUserCreationFaildException(string message) : base(message)
        {
           
        }
    }
}
