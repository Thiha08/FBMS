using FBMS.Core.Attributes;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace FBMS.Core.Dtos.Auth
{
    public class AuthRequest
    {
        public string AuthUrl { get; set; }

        public List<Cookie> Cookies { get; set; }

        public bool isSignedIn { get; set; }

        public SignInDto RequestForm { get; set; }
    }
}
