using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Email
{
    public interface IEmailSettings
    {
        string Host { get; set; }

        int Port { get; set; }

        string SenderName { get; set; }

        string SenderEmail { get; set; }

        string Password { get; set; }

        string Recipients { get; set; }
    }
}
