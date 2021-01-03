namespace FBMS.Core.Dtos.Filters
{
    public class MemberFilterDto : BaseFilterDto
    {
        public string UserName { get; set; }

        public bool? Status { get; set; }
    }
}
