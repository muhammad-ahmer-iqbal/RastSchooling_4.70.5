using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Students
{
    public partial record StudentLeaveModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Students.StudentLeaveModel.Field.CustomerId")]
        public int CustomerId { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Admin.Students.StudentLeaveModel.Field.StartDate")]
        public DateTime? StartDate { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Admin.Students.StudentLeaveModel.Field.EndDate")]
        public DateTime? EndDate { get; set; }
    }
}
