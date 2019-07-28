using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Domain.Exceptions
{
    public class InvalidUsernamePasswordException: UnauthorizedAccessException
    {
        public InvalidUsernamePasswordException(string message): base(message)
        {

        }
    }
}
