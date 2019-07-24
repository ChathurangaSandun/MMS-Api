using Accoon.MMS.Api.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Domain.Entities
{
    public class Customer : Entity<Guid>
    {
        public Customer()
        {
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}
