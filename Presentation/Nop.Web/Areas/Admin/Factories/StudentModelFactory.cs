using Microsoft.Identity.Client;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Students;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Students;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial record StudentModelFactory : IStudentModelFactory
    {
        #region Fields

        protected readonly ICustomerService _customerService;
        protected readonly IStudentExtensionService _studentExtensionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
        protected readonly ICurrencyService _currencyService;
        protected readonly CurrencySettings _currencySettings;
        protected readonly IStudentLeaveService _studentLeaveService;

        #endregion

        #region Ctor

        public StudentModelFactory(
            ICustomerService customerService,
            IStudentExtensionService studentExtensionService,
            ILocalizationService localizationService,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IStudentLeaveService studentLeaveService
            )
        {
            _customerService = customerService;
            _studentExtensionService = studentExtensionService;
            _localizationService = localizationService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _currencyService = currencyService;
            _currencySettings = currencySettings;
            _studentLeaveService = studentLeaveService;
        }

        #endregion

        #region Methods

        #region Student

        public virtual async Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            await _baseAdminModelFactory.PrepareActiveOptionsAsync(searchModel.AvailableActiveOptions);

            searchModel.SetGridPageSize();

            return searchModel;
        }
        public virtual async Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _customerService.GetAllCustomersAsync(
                customerRoleSystemNames: new[] { NopCustomerDefaults.StudentsRoleName },
                firstName: searchModel.FirstName,
                lastName: searchModel.LastName,
                isActive: searchModel.ActiveId > 0 ? searchModel.ActiveId.Equals(1) : null,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new StudentListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<StudentModel>();

                    var studentExtension = await _studentExtensionService.GetStudentExtensionByCustomerIdAsync(entity.Id);
                    tempModel = studentExtension.ToModel(tempModel);

                    return tempModel;
                });
            });

            return model;
        }
        public virtual async Task<StudentModel> PrepareStudentModelAsync(
            StudentModel model,
            Customer entity,
            bool excludeProperties = default
            )
        {
            if (entity != null)
            {
                //fill in model values from the entity
                model ??= entity.ToModel<StudentModel>();

                var studentExtension = await _studentExtensionService.GetStudentExtensionByCustomerIdAsync(entity.Id);
                model = studentExtension.ToModel(model);
            }
            else
            {
                if (!excludeProperties)
                {
                    model.Active = true;
                    model.DateOfAdmission = DateTime.Today;
                }
            }

            model.StudentLeaveSearchModel = await this.PrepareStudentLeaveSearchModelAsync(model.StudentLeaveSearchModel);

            return model;
        }

        #endregion

        #region Student Leave

        public virtual async Task<StudentLeaveSearchModel> PrepareStudentLeaveSearchModelAsync(StudentLeaveSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }
        public virtual async Task<StudentLeaveListModel> PrepareStudentLeaveListModelAsync(StudentLeaveSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _studentLeaveService.GetAllStudentLeavesAsync(
                customerId: searchModel.CustomerId,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new StudentLeaveListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<StudentLeaveModel>();

                    return tempModel;
                });
            });

            return model;
        }

        #endregion

        #endregion
    }
}
