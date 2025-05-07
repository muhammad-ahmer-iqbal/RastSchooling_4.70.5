using Nop.Web.Areas.Admin.Models.Staffs;

namespace Nop.Web.Areas.Admin.Factories
{
    public interface IDesignationModelFactory
    {
        Task<DesignationSearchModel> PrepareDesignationSearchModelAsync(DesignationSearchModel searchModel);

        Task<DesignationListModel> PrepareDesignationListModelAsync(DesignationSearchModel searchModel);
    }
}
