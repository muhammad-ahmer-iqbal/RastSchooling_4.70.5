using System.ComponentModel.DataAnnotations;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Students
{
    public partial record StudentModel : BaseNopEntityModel
    {
        public StudentModel()
        {
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

        public string PrimaryStoreCurrencyCode { get; set; }
    }
}
