using Microsoft.AspNetCore.Mvc;

namespace FBMS.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
    }
}
