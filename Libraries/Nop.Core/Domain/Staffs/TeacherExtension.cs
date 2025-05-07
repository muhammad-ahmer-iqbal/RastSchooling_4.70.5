namespace Nop.Core.Domain.Staffs
{
    public partial class TeacherExtension : BaseEntity
    {
        public int CustomerId { get; set; }
        public int Salary { get; set; }
        public int ShiftId { get; set; }
        public ShiftEnum Shift
        {
            get => (ShiftEnum)this.ShiftId;
            set { this.ShiftId = (int)value; }
        }
        public DateTime DateOfJoining { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
