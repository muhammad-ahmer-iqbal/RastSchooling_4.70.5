using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Forms;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Forms
{
    public partial record FormFieldModel : BaseNopEntityModel
    {
        public FormFieldModel()
        {
            AvailableControlTypes = new List<SelectListItem>();
            FormFieldOptionModels = new List<FormFieldOptionModel>();
        }

        public FormFieldModel(int formId) : this()
        {
            FormId = formId;
        }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.FormId")]
        public int FormId { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.Required")]
        public bool Required { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.ControlTypeId")]
        public int ControlTypeId { get; set; }
        public IList<SelectListItem> AvailableControlTypes { get; set; }
        public ControlTypeEnum ControlType
        {
            get => (ControlTypeEnum)this.ControlTypeId;
            set { this.ControlTypeId = (int)value; }
        }
        public string ControlTypeString { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormFieldModel.Field.Options")]
        public string Options { get; set; }

        public IList<FormFieldOptionModel> FormFieldOptionModels { get; set; }
    }
}
