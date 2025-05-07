using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Staffs
{
    public partial record DepartmentSearchModel : BaseSearchModel
    {
        public DepartmentSearchModel()
        {
            DepartmentModel = new DepartmentModel();
        }
        public DepartmentModel DepartmentModel { get; set; }
    }
}
