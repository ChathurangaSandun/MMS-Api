using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.Notification.Model
{
    public class Message
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
