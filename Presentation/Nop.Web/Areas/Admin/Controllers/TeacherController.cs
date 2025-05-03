using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Staffs;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Staffs;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Models.Staffs;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using ILogger = Nop.Services.Logging.ILogger;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class TeacherController : BaseAdminController
    {
        #region Fields

        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;
        protected readonly ITeacherModelFactory _teacherModelFactory;
        protected readonly ICustomerService _customerService;
        protected readonly ITeacherExtensionService _teacherExtensionService;
        protected readonly ILogger _logger;
        protected readonly CustomerSettings _customerSettings;
        protected readonly ICustomerRegistrationService _customerRegistrationService;

        #endregion

        #region Ctor

        public TeacherController(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ITeacherModelFactory teacherModelFactory,
            ICustomerService customerService,
            ITeacherExtensionService teacherExtensionService,
            ILogger logger,
            CustomerSettings customerSettings,
            ICustomerRegistrationService customerRegistrationService
            )
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _teacherModelFactory = teacherModelFactory;
            _customerService = customerService;
            _teacherExtensionService = teacherExtensionService;
            _logger = logger;
            _customerSettings = customerSettings;
            _customerRegistrationService = customerRegistrationService;
        }

        #endregion

        #region Utilities

        private async Task ValidatorAsync(TeacherModel model)
        {
            ArgumentNullException.ThrowIfNull(nameof(model));

            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError(nameof(model.FirstName), await _localizationService.GetResourceAsync("Admin.Teachers.TeacherModel.Fields.FirstName.Required"));
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError(nameof(model.LastName), await _localizationService.GetResourceAsync("Admin.Teachers.TeacherModel.Fields.LastName.Required"));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), await _localizationService.GetResourceAsync("Admin.Teachers.TeacherModel.Fields.Email.Required"));
            }
            else if (!CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), await _localizationService.GetResourceAsync("Admin.Teachers.TeacherModel.Fields.Email.InValid"));
            }
            else if (!model.Id.Equals(default)
                && await _customerService.GetCustomerByEmailAsync(model.Email) is var existingCustomer
                && existingCustomer is not null &&
                !existingCustomer.Id.Equals(model.Id))
            {
                ModelState.AddModelError(nameof(model.Email), await _localizationService.GetResourceAsync("Admin.Teachers.TeacherModel.Fields.Email.AlreadyInUse"));
            }
        }

        private async Task<Customer> InsertUpdateCustomerAsync(TeacherModel model, Customer entity)
        {
            entity ??= new Customer()
            {
                CreatedOnUtc = DateTime.UtcNow,
            };
            entity = model.ToEntity(entity);
            await _customerService.InsertUpdateCustomerAsync(entity);

            var teacherExtension = await _teacherExtensionService.GetTeacherExtensionByCustomerIdAsync(entity.Id)
                ?? new TeacherExtension()
                {
                    CustomerId = entity.Id
                };
            teacherExtension = model.ToEntity(teacherExtension);
            await _teacherExtensionService.InsertUpdateTeacherExtensionAsync(teacherExtension);

            var teacherRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.TeachersRoleName);
            var existingRole = await _customerService.GetCustomerRolesAsync(entity);
            if (!existingRole.Any(x => x.Id == teacherRole.Id))
            {
                //already assigned
                await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping()
                {
                    CustomerId = entity.Id,
                    CustomerRoleId = teacherRole.Id,
                });
            }

            return entity;
        }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            var model = await _teacherModelFactory.PrepareTeacherSearchModelAsync(new TeacherSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(TeacherSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return await AccessDeniedDataTablesJson();

            var model = await _teacherModelFactory.PrepareTeacherListModelAsync(searchModel);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            var model = await _teacherModelFactory.PrepareTeacherModelAsync(new TeacherModel(), null);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(TeacherModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            await ValidatorAsync(model);

            if (ModelState.IsValid)
            {
                try
                {
                    //insert entity
                    var entity = await InsertUpdateCustomerAsync(model, default);

                    //success notification
                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Common.Added"));
                    return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    await _logger.ErrorAsync(ex.Message, ex);
                }
            }

            model = await _teacherModelFactory.PrepareTeacherModelAsync(model, null);
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            var entity = await _customerService.GetTeacherByIdAsync(id);
            if (entity == null)
                return RedirectToAction("List");

            var model = await _teacherModelFactory.PrepareTeacherModelAsync(null, entity);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Edit(TeacherModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            var entity = await _customerService.GetTeacherByIdAsync(model.Id);
            if (entity == null)
                return RedirectToAction("List");

            await ValidatorAsync(model);

            if (ModelState.IsValid)
            {
                try
                {
                    //update entity
                    entity = await InsertUpdateCustomerAsync(model, entity);

                    //success notification
                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Common.Updated"));
                    return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    await _logger.ErrorAsync(ex.Message, ex);
                }
            }

            //if we got this far, something failed, redisplay form
            model = await _teacherModelFactory.PrepareTeacherModelAsync(model, entity);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var entity = await _customerService.GetTeacherByIdAsync(id);
            if (entity == null)
                return RedirectToAction("List");

            try
            {
                //delete customer
                await _customerService.DeleteCustomerAsync(entity);

                //success notification
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Common.Deleted"));
            }
            catch (Exception ex)
            {
                //error notification
                _notificationService.ErrorNotification(ex.Message);
                await _logger.ErrorAsync(ex.Message, ex);
            }

            return RedirectToAction("List");
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public virtual async Task<IActionResult> ChangePassword(CustomerModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageTeachers))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var customer = await _customerService.GetTeacherByIdAsync(model.Id);
            if (customer == null)
                return RedirectToAction("List");

            var changePassRequest = new ChangePasswordRequest(customer.Email,
                false, _customerSettings.DefaultPasswordFormat, model.Password);
            var changePassResult = await _customerRegistrationService.ChangePasswordAsync(changePassRequest);
            if (changePassResult.Success)
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Common.PasswordChanged"));
            else
                foreach (var error in changePassResult.Errors)
                    _notificationService.ErrorNotification(error);

            return RedirectToAction("Edit", new { id = customer.Id });
        }


        #endregion
    }
}
