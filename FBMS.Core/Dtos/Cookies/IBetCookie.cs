using FBMS.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBMS.Core.Dtos.Cookies
{
    public class IBetCookie : ICookie
    {
        [Description("ASP.NET_SessionId")]
        public string AspNetSessionId { get; set; }

        [Description("BPX-STICKY-SESSION")]
        public string BpxStickySession { get; set; }

        [Description(".ASPXAUTH")]
        public string AspxAuth { get; set; }

    }
}
