using Nop.Core.Domain.Directory;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Staffs;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Staffs;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial record DesignationModelFactory : IDesignationModelFactory
    {
        #region Fields

        protected readonly ICustomerService _customerService;
        protected readonly ITeacherExtensionService _teacherExtensionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
        protected readonly ICurrencyService _currencyService;
        protected readonly CurrencySettings _currencySettings;
        protected readonly IDesignationService _designationService;
        protected readonly IDesignationService _DesignationService;

        #endregion

        #region Ctor

        public DesignationModelFactory(
            ICustomerService customerService,
            ITeacherExtensionService teacherExtensionService,
            ILocalizationService localizationService,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICurrencyService currencyService,
            CurrencySettings currencySettings
            )
        {
            _customerService = customerService;
            _teacherExtensionService = teacherExtensionService;
            _localizationService = localizationService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _currencyService = currencyService;
            _currencySettings = currencySettings;
        }

        #endregion

        #region Methods

        public virtual async Task<DesignationSearchModel> PrepareDesignationSearchModelAsync(DesignationSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }
        public virtual async Task<DesignationListModel> PrepareDesignationListModelAsync(DesignationSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _DesignationService.GetAllDesignationsAsync(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new DesignationListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<DesignationModel>();

                    return tempModel;
                });
            });

            return model;
        }

        #endregion
    }
}
