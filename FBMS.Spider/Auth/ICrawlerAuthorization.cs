using FBMS.Core.Dtos.Auth;
using System.Threading.Tasks;

namespace FBMS.Spider.Auth
{
    public interface ICrawlerAuthorization
    {
        Task<AuthResponse> IsSignedInAsync(string baseUrl);

        Task<AuthResponse> SignInAsync(AuthRequest request);
    }
}
