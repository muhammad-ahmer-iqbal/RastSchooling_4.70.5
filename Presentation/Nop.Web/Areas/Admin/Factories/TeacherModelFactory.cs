using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Staffs;
using Nop.Services;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Staffs;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Staffs;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial record TeacherModelFactory : ITeacherModelFactory
    {
        #region Fields

        protected readonly ICustomerService _customerService;
        protected readonly ITeacherExtensionService _teacherExtensionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
        protected readonly ICurrencyService _currencyService;
        protected readonly CurrencySettings _currencySettings;
        protected readonly IDesignationService _designationService;
        protected readonly IDepartmentService _departmentService;

        #endregion

        #region Ctor

        public TeacherModelFactory(
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

        public virtual async Task<TeacherSearchModel> PrepareTeacherSearchModelAsync(TeacherSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            await _baseAdminModelFactory.PrepareActiveOptionsAsync(searchModel.AvailableActiveOptions);

            searchModel.SetGridPageSize();

            return searchModel;
        }
        public virtual async Task<TeacherListModel> PrepareTeacherListModelAsync(TeacherSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _customerService.GetAllCustomersAsync(
                customerRoleSystemNames: new[] { NopCustomerDefaults.TeachersRoleName },
                firstName: searchModel.FirstName,
                lastName: searchModel.LastName,
                isActive: searchModel.ActiveId > 0 ? searchModel.ActiveId.Equals(1) : null,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new TeacherListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<TeacherModel>();

                    var teacherExtension = await _teacherExtensionService.GetTeacherExtensionByCustomerIdAsync(entity.Id);
                    tempModel = teacherExtension.ToModel(tempModel);

                    return tempModel;
                });
            });

            return model;
        }
        public virtual async Task<TeacherModel> PrepareTeacherModelAsync(
            TeacherModel model,
            Customer entity,
            bool excludeProperties = default
            )
        {
            if (entity != null)
            {
                //fill in model values from the entity
                model ??= entity.ToModel<TeacherModel>();

                var teacherExtension = await _teacherExtensionService.GetTeacherExtensionByCustomerIdAsync(entity.Id);
                model = teacherExtension.ToModel(model);
            }
            else
            {
                if (!excludeProperties)
                {
                    model.Active = true;
                    model.DateOfJoining = DateTime.Today;
                }
            }

            var allDepartments = await _departmentService.GetAllDepartmentsAsync();
            var allDesignations = await _designationService.GetAllDesignationsAsync();

            model.AvailableShifts = (await ShiftEnum.Morning.ToSelectListAsync()).ToList();
            model.AvailableDepartments = allDepartments.ToSelectList(x => (x as Department).Name).ToList();
            model.AvailableDesignations = allDesignations.ToSelectList(x => (x as Department).Name).ToList();

            var defaultItem = new SelectListItem(text: await _localizationService.GetResourceAsync("admin.common.select"), value: "0");
            model.AvailableDepartments.Insert(0, defaultItem);
            model.AvailableDesignations.Insert(0, defaultItem);

            return model;
        }

        #endregion
    }
}
