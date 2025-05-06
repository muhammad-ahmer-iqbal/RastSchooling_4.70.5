using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Students
{
    public partial record StudentLeaveSearchModel : BaseSearchModel
    {
        public int CustomerId { get; set; }
    }
}
