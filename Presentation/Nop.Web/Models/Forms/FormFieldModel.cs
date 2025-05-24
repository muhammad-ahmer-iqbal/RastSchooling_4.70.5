using Nop.Core.Domain.Forms;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Forms
{
    public partial record FormFieldModel : BaseNopEntityModel
    {
        public FormFieldModel()
        {
            Options = new List<FormFieldOptionModel>();
        }
        public string Name { get; set; }
        public ControlTypeEnum ControlType { get; set; }
        public bool Required { get; set; }
        public IList<FormFieldOptionModel> Options { get; set; }
    }
}
