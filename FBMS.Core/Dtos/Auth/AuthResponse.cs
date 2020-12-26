using System;
using System.Collections.Generic;
using System.Net;

namespace FBMS.Core.Dtos.Auth
{
    public class AuthResponse
    {
        public Uri BaseUrl { get; set; }

        public List<Cookie> Cookies { get; set; } = new List<Cookie>();

        public string HtmlCode { get; set; }

        public bool isSignedIn { get; set; }
    }
}
