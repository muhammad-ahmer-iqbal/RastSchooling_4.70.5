using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Forms
{
    public partial record FormFieldOptionModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Forms.FormFieldOptionModel.Field.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldOptionModel.Field.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldOptionModel.Field.FormFieldId")]
        public int FormFieldId { get; set; }
    }
}
