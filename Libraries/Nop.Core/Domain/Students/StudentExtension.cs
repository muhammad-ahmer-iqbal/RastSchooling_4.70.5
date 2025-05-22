namespace Nop.Core.Domain.Students
{
    public partial class StudentExtension : BaseEntity
    {
        public int CustomerId { get; set; }
        public int MonthlyFee { get; set; }
        public DateTime? DateOfAdmission { get; set; }
        public int? HouseId { get; set; }
        public HouseEnum House
        {
            get => (HouseEnum)this.HouseId;
            set { this.HouseId = (int)value; }
        }
    }
}
