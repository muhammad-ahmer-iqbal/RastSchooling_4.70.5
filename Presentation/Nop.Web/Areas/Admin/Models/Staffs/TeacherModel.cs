using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Staffs
{
    public partial record TeacherModel : BaseNopEntityModel
    {
        public TeacherModel()
        {
            AvailableShifts = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Staffs.TeacherModel.Fields.Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherModel.Fields.FirstName")]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherModel.Fields.LastName")]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherModel.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherModel.Fields.ShiftId")]
        public int ShiftId { get; set; }
        public IList<SelectListItem> AvailableShifts { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherModel.Fields.Salary")]
        public int Salary { get; set; }

        [NopResourceDisplayName("Admin.Common.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
