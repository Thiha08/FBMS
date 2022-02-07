using System.ComponentModel.DataAnnotations;

namespace FBMS.Core.Dtos
{
    public class SettingDto : BaseDto
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Key")]
        [Required]
        public string Key { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
