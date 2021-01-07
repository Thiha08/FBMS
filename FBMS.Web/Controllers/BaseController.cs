using FBMS.Web.ViewExtensions;
using FBMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FBMS.Web.Controllers
{
    public class BaseController : Controller
    {
        internal void GenerateAlertMessage(bool isSuccessful, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var alertViewModel = new AlertViewModel
                {
                    Success = isSuccessful,
                    Message = message
                };

                TempData.Put("AlertViewModel", alertViewModel);
            }
        }
    }
}
