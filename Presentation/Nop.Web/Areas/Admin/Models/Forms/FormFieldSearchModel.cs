using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Forms
{
    public partial record FormFieldSearchModel : BaseSearchModel
    {
        public int FormId { get; set; }
    }
}
