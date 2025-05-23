using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Students
{
    public partial record StudentModel : BaseNopEntityModel
    {
        public StudentModel()
        {
            StudentLeaveSearchModel = new StudentLeaveSearchModel();
            StudentLeaveModel = new StudentLeaveModel();
            AvailableSessions = new List<SelectListItem>();
            SelectedSessionIds = new List<int>();
            AvailableHouses = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.MonthlyFee")]
        public int MonthlyFee { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.FirstName")]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.LastName")]
        public string LastName { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Common.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.DateOfAdmission")]
        public DateTime? DateOfAdmission { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.SelectedSessionIds")]
        public IList<int> SelectedSessionIds { get; set; }
        public IList<SelectListItem> AvailableSessions { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentModel.Fields.HouseId")]
        public int? HouseId { get; set; }
        public IList<SelectListItem> AvailableHouses { get; set; }

        [NopResourceDisplayName("Admin.Common.DefaultPasword")]
        public string CustomerGuid { get; set; }



        public StudentLeaveSearchModel StudentLeaveSearchModel { get; set; }
        public StudentLeaveModel StudentLeaveModel { get; set; }

    }
}
