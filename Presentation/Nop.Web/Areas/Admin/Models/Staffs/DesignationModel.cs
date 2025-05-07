using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Staffs
{
    public partial record DesignationModel : BaseNopEntityModel
    {
        public string Name { get; set; }
    }
}
