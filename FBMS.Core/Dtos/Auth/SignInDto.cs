using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System.ComponentModel;

namespace FBMS.Core.Dtos.Auth
{
    public class SignInDto : BaseEntity, IAggregateRoot
    {
        [Description("__EVENTTARGET")]
        public string EventTarget { get; set; }

        [Description("__EVENTARGUMENT")]
        public string EventArgument { get; set; }

        [Description("__EVENTVALIDATION")]
        public string EventValidation { get; set; }

        [Description("__VIEWSTATE")]
        public string ViewState { get; set; }

        [Description("__VIEWSTATEGENERATOR")]
        public string ViewStateGenerator { get; set; }

        [Description("txtUserName")]
        public string TxtUserName { get; set; }

        [Description("txtPassword")]
        public string TxtPassword { get; set; }

        [Description("password")]
        public string Password { get; set; }
    }
}
