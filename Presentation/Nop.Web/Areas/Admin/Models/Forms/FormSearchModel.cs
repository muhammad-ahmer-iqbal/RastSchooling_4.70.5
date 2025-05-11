using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Forms
{
    public partial record FormSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Admin.Forms.FormSearchModel.Field.Name")]
        public string Name { get; set; }
    }
}
