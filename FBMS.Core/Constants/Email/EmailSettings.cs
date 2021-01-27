using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Email
{
    public class EmailSettings : IEmailSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }

        public string Password { get; set; }

        public string Recipients { get; set; }
    }
}
