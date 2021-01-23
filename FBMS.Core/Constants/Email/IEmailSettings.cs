using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Email
{
    public interface IEmailSettings
    {
        bool IsDevelopment { get; set; }

        bool UseSsl { get; set; }

        string MailServer { get; set; }

        int MailPort { get; set; }

        string SenderEmail { get; set; }

        string SenderName { get; set; }

        string Password { get; set; }

        string AdminEmail { get; set; }
    }
}
