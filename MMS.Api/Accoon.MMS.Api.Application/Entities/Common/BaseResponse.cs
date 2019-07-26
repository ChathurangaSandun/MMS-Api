using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.Entities.Common
{
    public abstract class BaseResponse
    {
        public bool Success { get; set; }
        public IEquatable<string> Errors { get; set; }
    }
}
