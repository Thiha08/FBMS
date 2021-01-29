using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Mail
{
    public interface IEmailTemplateProvider
    {
        string GetTransactionCompletedEmailTemplate();

        string GetTransactionDischargedEmailTemplate();
    }
}
