using FBMS.Core.Extensions;
using FBMS.Core.Mail;
using System.Text;

namespace FBMS.Infrastructure.Mail
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        public string GetTransactionCompletedEmailTemplate()
        {
            using (var stream = typeof(IEmailTemplateProvider).Assembly.GetManifestResourceStream("FBMS.Core.Mail.EmailTemplates.transaction-completed-email-template.html"))
            {
                var bytes = stream.GetAllBytes();
                var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                return template;
            }
        }
    }
}
