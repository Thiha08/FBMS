﻿using Newtonsoft.Json;

namespace FBMS.Web.ViewModels
{
    public class ExceptionViewModel
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
