using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Staffs;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface ITeacherModelFactory
    {
        Task<TeacherSearchModel> PrepareTeacherSearchModelAsync(TeacherSearchModel searchModel);
        Task<TeacherListModel> PrepareTeacherListModelAsync(TeacherSearchModel searchModel);
        Task<TeacherModel> PrepareTeacherModelAsync(
            TeacherModel model,
            Customer entity,
            bool excludeProperties = default
            );
    }
}
