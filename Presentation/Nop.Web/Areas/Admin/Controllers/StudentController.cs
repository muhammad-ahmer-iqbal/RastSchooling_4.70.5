using Microsoft.AspNetCore.Mvc;

using Nop.Core;
using Nop.Core.Domain.Utilities;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Students;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Students;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Models.Students;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

using ILogger = Nop.Services.Logging.ILogger;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class StudentController : BaseAdminController
    {
        #region Fields

        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;
        protected readonly IStudentModelFactory _studentModelFactory;
        protected readonly ICustomerService _customerService;
        protected readonly IStudentExtensionService _studentExtensionService;
        protected readonly ILogger _logger;
        protected readonly CustomerSettings _customerSettings;
        protected readonly ICustomerRegistrationService _customerRegistrationService;
        protected readonly IStudentLeaveService _studentLeaveService;
        protected readonly IStudentSessionMappingService _studentSessionMappingService;

        #endregion

        #region Ctor

        public StudentController(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IStudentModelFactory studentModelFactory,
            ICustomerService customerService,
            IStudentExtensionService studentExtensionService,
            ILogger logger,
            CustomerSettings customerSettings,
            ICustomerRegistrationService customerRegistrationService,
            IStudentLeaveService studentLeaveService,
            IStudentSessionMappingService studentSessionMappingService
            )
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _studentModelFactory = studentModelFactory;
            _customerService = customerService;
            _studentExtensionService = studentExtensionService;
            _logger = logger;
            _customerSettings = customerSettings;
            _customerRegistrationService = customerRegistrationService;
            _studentLeaveService = studentLeaveService;
            _studentSessionMappingService = studentSessionMappingService;
        }

        #endregion

        #region Utilities

        private async Task ValidatorAsync(StudentModel model)
        {
            ArgumentNullException.ThrowIfNull(nameof(model));

            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError(nameof(model.FirstName), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.FirstName.Required"));
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError(nameof(model.LastName), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.LastName.Required"));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.Email.Required"));
            }
            else if (!CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.Email.InValid"));
            }
            else if (await _customerService.GetCustomerByEmailAsync(model.Email) is var existingCustomer
                && existingCustomer is not null
                && (model.Id.Equals(default)
                    || !model.Id.Equals(existingCustomer.Id)))
            {
                ModelState.AddModelError(nameof(model.Email), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.Email.AlreadyInUse"));
            }

            if (model.DateOfBirth.HasValue
                && model.DateOfBirth.Value.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.DateOfBirth.InValid"));
            }

            if (!model.DateOfAdmission.HasValue)
            {
                ModelState.AddModelError(nameof(model.DateOfAdmission), await _localizationService.GetResourceAsync("Admin.Students.StudentModel.Fields.DateOfAdmission.Required"));
            }
        }

        private async Task<Customer> InsertUpdateCustomerAsync(StudentModel model, Customer entity)
        {
            #region Save Entity Data

            entity ??= new Customer()
            {
                CreatedOnUtc = DateTime.UtcNow,
            };
            entity = model.ToEntity(entity);
            await _customerService.InsertUpdateCustomerAsync(entity);

            var studentExtension = await _studentExtensionService.GetStudentExtensionByCustomerIdAsync(entity.Id)
                ?? new StudentExtension()
                {
                    CustomerId = entity.Id
                };
            studentExtension = model.ToEntity(studentExtension);
            await _studentExtensionService.InsertUpdateStudentExtensionAsync(studentExtension);

            #endregion

            #region Add Roles

            var studentRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.StudentsRoleName);
            var registerRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.RegisteredRoleName);

            var existingRole = await _customerService.GetCustomerRolesAsync(entity);
            if (!existingRole.Any(x => x.Id == studentRole.Id))
            {
                //already assigned
                await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping()
                {
                    CustomerId = entity.Id,
                    CustomerRoleId = studentRole.Id,
                });
            }
            if (!existingRole.Any(x => x.Id == registerRole.Id))
            {
                //already assigned
                await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping()
                {
                    CustomerId = entity.Id,
                    CustomerRoleId = registerRole.Id,
                });
            }

            #endregion

            #region Add Sessions

            var existingSessions = await _studentSessionMappingService.GetAllStudentSessionMappingsAsync(customerId: entity.Id);
            var insertSessions = new List<StudentSessionMapping>();
            if (model.SelectedSessionIds is not null)
            {
                foreach (var sessionId in model.SelectedSessionIds)
                {
                    if (existingSessions.FirstOrDefault(x => x.DepartmentId.Equals(sessionId)) is var existingSession
                        && existingSession is not null)
                    {
                        existingSessions.Remove(existingSession); //remove from existing session list
                        //already assigned
                        continue;
                    }

                    insertSessions.Add(new StudentSessionMapping()
                    {
                        CustomerId = entity.Id,
                        DepartmentId = sessionId
                    });
                }
            }

            await _studentSessionMappingService.InsertStudentSessionMappingAsync(insertSessions);
            await _studentSessionMappingService.DeleteStudentSessionMappingAsync(existingSessions);

            #endregion


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
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            var model = await _studentModelFactory.PrepareStudentSearchModelAsync(new StudentSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(StudentSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return await AccessDeniedDataTablesJson();

            var model = await _studentModelFactory.PrepareStudentListModelAsync(searchModel);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            var model = await _studentModelFactory.PrepareStudentModelAsync(new StudentModel(), null);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(StudentModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            await ValidatorAsync(model);

            if (ModelState.IsValid)
            {
                try
                {
                    //insert entity
                    var entity = await InsertUpdateCustomerAsync(model, default);

                    #region Set Default Password

                    var changePassRequest = new ChangePasswordRequest(
                        email: entity.Email,
                        validateRequest: false,
                        newPasswordFormat: _customerSettings.DefaultPasswordFormat,
                        newPassword: entity.CustomerGuid.ToString());
                    var changePassResult = await _customerRegistrationService.ChangePasswordAsync(changePassRequest);
                    if (!changePassResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, await _localizationService.GetResourceAsync("Admin.Common.Password.CouldNotBeSet"));
                    }

                    #endregion

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

            model = await _studentModelFactory.PrepareStudentModelAsync(model, null, true);
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id, bool viewMode = default)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            var entity = await _customerService.GetStudentByIdAsync(id);
            if (entity == null)
                return RedirectToAction("List");

            ViewBag.ViewMode = viewMode;
            var model = await _studentModelFactory.PrepareStudentModelAsync(null, entity);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Edit(StudentModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            var entity = await _customerService.GetStudentByIdAsync(model.Id);
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
            model = await _studentModelFactory.PrepareStudentModelAsync(model, entity, true);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var entity = await _customerService.GetStudentByIdAsync(id);
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
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            //try to get a customer with the specified id
            var entity = await _customerService.GetStudentByIdAsync(model.Id);
            if (entity == null)
                return RedirectToAction("List");

            var changePassRequest = new ChangePasswordRequest(
                email: entity.Email,
                validateRequest: false,
                newPasswordFormat: _customerSettings.DefaultPasswordFormat,
                newPassword: model.Password);
            var changePassResult = await _customerRegistrationService.ChangePasswordAsync(changePassRequest);
            if (changePassResult.Success)
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Common.PasswordChanged"));
            else
                foreach (var error in changePassResult.Errors)
                    _notificationService.ErrorNotification(error);

            return RedirectToAction("Edit", new { id = entity.Id });
        }

        #region Student Leave

        [HttpPost]
        public virtual async Task<IActionResult> StudentLeaveList(StudentLeaveSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _studentModelFactory.PrepareStudentLeaveListModelAsync(searchModel);
            return Json(model);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public virtual async Task<IActionResult> StudentLeaveAdd(StudentLeaveModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return AccessDeniedView();

            try
            {
                if (model.CustomerId.Equals(default))
                    throw new ArgumentNullException(nameof(model.CustomerId));
                else if (!model.StartDate.HasValue
                    || !model.EndDate.HasValue)
                {
                    throw new ArgumentNullException("Start and End Date is required");
                }

                var entity = model.ToEntity<StudentLeave>();
                await _studentLeaveService.InsertStudentLeaveAsync(entity);

                return Ok(new ApiResponseModel(success: true, message: await _localizationService.GetResourceAsync("Admin.Common.Added")));
            }
            catch (Exception exc)
            {
                await _logger.ErrorAsync(exc.Message, exc);
                return Ok(new ApiResponseModel(success: false, message: exc.Message));
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> StudentLeaveDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return await AccessDeniedDataTablesJson();

            var entity = await _studentLeaveService.GetStudentLeaveByIdAsync(id)
                ?? throw new ArgumentException("No record found with the specified id");

            await _studentLeaveService.DeleteStudentLeaveAsync(entity);

            return new NullJsonResult();
        }

        #endregion

        #endregion
    }
}
