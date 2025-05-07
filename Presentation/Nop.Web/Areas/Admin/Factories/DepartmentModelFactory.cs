using Microsoft.AspNetCore.Mvc.Rendering;

using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Staffs;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Staffs;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Staffs;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class DepartmentModelFactory : IDepartmentModelFactory
    {
        #region Fields

        protected readonly ICustomerService _customerService;
        protected readonly ITeacherExtensionService _teacherExtensionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
        protected readonly ICurrencyService _currencyService;
        protected readonly CurrencySettings _currencySettings;
        protected readonly IDepartmentService _departmentService;

        #endregion

        #region Ctor

        public DepartmentModelFactory(
            ICustomerService customerService,
            ITeacherExtensionService teacherExtensionService,
            ILocalizationService localizationService,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IDepartmentService departmentService
            )
        {
            _customerService = customerService;
            _teacherExtensionService = teacherExtensionService;
            _localizationService = localizationService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _currencyService = currencyService;
            _currencySettings = currencySettings;
            _departmentService = departmentService;
        }

        #endregion

        #region Methods

        public virtual async Task<DepartmentSearchModel> PrepareDepartmentSearchModelAsync(DepartmentSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }
        public virtual async Task<DepartmentListModel> PrepareDepartmentListModelAsync(DepartmentSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _departmentService.GetAllDepartmentsAsync(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new DepartmentListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<DepartmentModel>();

                    return tempModel;
                });
            });

            return model;
        }

        #endregion
    }
}
