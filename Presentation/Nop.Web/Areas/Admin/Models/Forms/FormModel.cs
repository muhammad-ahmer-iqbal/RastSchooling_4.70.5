using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Forms;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Forms
{
    public partial record FormModel : BaseNopEntityModel
    {
        public FormModel()
        {
            FormFieldSearchModel = new FormFieldSearchModel();
            AvailableDesignations = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Forms.FormModel.Field.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormModel.Field.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormModel.Field.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.Forms.FormModel.Field.DepartmentId")]
        public int DepartmentId { get; set; }
        public IList<SelectListItem> AvailableDesignations { get; set; }



        public FormFieldSearchModel FormFieldSearchModel { get; set; }
    }
}
