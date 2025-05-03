using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Staffs
{
    public partial record TeacherSearchModel : BaseSearchModel
    {
        public TeacherSearchModel()
        {
            AvailableActiveOptions = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Staffs.TeacherSearchModel.Field.FirstName")]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherSearchModel.Field.LastName")]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Staffs.TeacherSearchModel.Field.Active")]
        public int ActiveId { get; set; }
        public IList<SelectListItem> AvailableActiveOptions { get; set; }
    }
}
