using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Staffs
{
    public partial record DepartmentModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Staffs.Departments.Fields.Name")]
        public string Name { get; set; }
    }
}
