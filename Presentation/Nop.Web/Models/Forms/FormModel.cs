using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Forms
{
    public partial record FormModel : BaseNopEntityModel
    {
        public FormModel()
        {
            FormFields = new List<FormFieldModel>();
        }
        public string Name { get; set; }
        public IList<FormFieldModel> FormFields { get; set; }
    }
}
