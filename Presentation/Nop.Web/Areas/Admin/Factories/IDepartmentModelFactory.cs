using Nop.Web.Areas.Admin.Models.Staffs;

namespace Nop.Web.Areas.Admin.Factories
{
    public interface IDepartmentModelFactory
    {
        Task<DepartmentSearchModel> PrepareDepartmentSearchModelAsync(DepartmentSearchModel searchModel);

        Task<DepartmentListModel> PrepareDepartmentListModelAsync(DepartmentSearchModel searchModel);
    }
}
