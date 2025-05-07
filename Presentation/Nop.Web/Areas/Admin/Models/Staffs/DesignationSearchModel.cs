using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Staffs
{
    public partial record DesignationSearchModel : BaseSearchModel
    {
        public DesignationSearchModel()
        {
            DesignationModel = new DesignationModel();
        }
        public DesignationModel DesignationModel { get; set; }
    }
}
