using System.ComponentModel.DataAnnotations;

namespace FBMS.Core.Dtos
{
    public class MemberDto : BaseDto
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
