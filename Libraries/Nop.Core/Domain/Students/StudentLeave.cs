namespace Nop.Core.Domain.Students
{
    public partial class StudentLeave : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
