using MimeKit;
using System.Threading.Tasks;

namespace FBMS.Core.Mail
{
    public interface IEmailSender
    {
        Task SendAsync(MimeMessage mail);
    }
}
