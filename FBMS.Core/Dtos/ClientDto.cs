using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FBMS.Core.Dtos
{
    public class ClientDto : BaseDto
    {
        [Display(Name = "Account")]
        public string Account { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
