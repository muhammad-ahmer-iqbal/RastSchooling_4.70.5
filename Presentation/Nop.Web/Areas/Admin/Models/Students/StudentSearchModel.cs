using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Students
{
    public partial record StudentSearchModel : BaseSearchModel
    {
        public StudentSearchModel()
        {
            AvailableActiveOptions = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Students.StudentSearchModel.Field.FirstName")]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentSearchModel.Field.LastName")]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Students.StudentSearchModel.Field.Active")]
        public int ActiveId { get; set; }
        public IList<SelectListItem> AvailableActiveOptions { get; set; }
    }
}
