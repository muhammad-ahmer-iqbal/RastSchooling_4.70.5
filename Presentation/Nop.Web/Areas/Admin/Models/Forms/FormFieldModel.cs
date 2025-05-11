using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Forms;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Forms
{
    public partial record FormFieldModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.FormId")]
        public int FormId { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.Required")]
        public bool Required { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.ControlType")]
        public int ControlTypeId { get; set; }
        public IList<SelectListItem> AvailableControlTypes { get; set; }
        public ControlTypeEnum ControlType
        {
            get => (ControlTypeEnum)this.ControlTypeId;
            set { this.ControlTypeId = (int)value; }
        }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.Options")]
        public string Options { get; set; }
    }
}
