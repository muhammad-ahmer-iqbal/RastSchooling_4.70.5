using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Utilities;
using Nop.Core.Domain.Forms;
using Nop.Services.Forms;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Forms;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

using ILogger = Nop.Services.Logging.ILogger;
using Nop.Services.Seo;
using Nop.Core;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class FormController : BaseAdminController
    {
        #region Fields

        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;
        protected readonly IFormModelFactory _formModelFactory;
        protected readonly IFormService _formService;
        protected readonly IFormFieldService _formFieldService;
        protected readonly IFormFieldOptionService _formFieldOptionService;
        protected readonly ILogger _logger;
        protected readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Ctor

        public FormController(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IFormModelFactory formModelFactory,
            IFormService formService,
            IFormFieldService formFieldService,
            IFormFieldOptionService formFieldOptionService,
            ILogger logger,
            IUrlRecordService urlRecordService
            )
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _formModelFactory = formModelFactory;
            _formService = formService;
            _formFieldService = formFieldService;
            _formFieldOptionService = formFieldOptionService;
            _logger = logger;
            _urlRecordService = urlRecordService;
        }

        #endregion

        #region Utilities

        private async Task ValidatorAsync(FormModel model)
        {
            ArgumentNullException.ThrowIfNull(nameof(model));

            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), await _localizationService.GetResourceAsync("Admin.Forms.FormModel.Field.Name.Required"));
            }
        }

        private async Task<Form> InsertUpdateFormAsync(FormModel model, Form entity)
        {
            entity ??= new Form();
            entity = model.ToEntity(entity);
            await _formService.InsertUpdateFormAsync(entity);

            var seName = await _urlRecordService.ValidateSeNameAsync(
                entity: entity,
                seName: model.Name,
                name: entity.Name,
                ensureNotEmpty: true);

            await _urlRecordService.SaveSlugAsync(
                entity: entity,
                slug: seName);

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
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            var model = await _formModelFactory.PrepareFormSearchModelAsync(new FormSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(FormSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return await AccessDeniedDataTablesJson();

            var model = await _formModelFactory.PrepareFormListModelAsync(searchModel);
            return Json(model);
        }

        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            var model = await _formModelFactory.PrepareFormModelAsync(new FormModel(), null);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Create(FormModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            await ValidatorAsync(model);

            if (ModelState.IsValid)
            {
                try
                {
                    //insert entity
                    var entity = await InsertUpdateFormAsync(model, default);

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

            model = await _formModelFactory.PrepareFormModelAsync(model, null);
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            var entity = await _formService.GetFormByIdAsync(id);
            if (entity == null)
                return RedirectToAction("List");

            var model = await _formModelFactory.PrepareFormModelAsync(null, entity);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual async Task<IActionResult> Edit(FormModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            var entity = await _formService.GetFormByIdAsync(model.Id);
            if (entity == null)
                return RedirectToAction("List");

            await ValidatorAsync(model);

            if (ModelState.IsValid)
            {
                try
                {
                    //update entity
                    entity = await InsertUpdateFormAsync(model, entity);

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
            model = await _formModelFactory.PrepareFormModelAsync(model, entity);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            //try to get a form with the specified id
            var entity = await _formService.GetFormByIdAsync(id);
            if (entity == null)
                return RedirectToAction("List");

            try
            {
                //delete form
                await _formService.DeleteFormAsync(entity);

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


        #region Form Field

        [HttpPost]
        public virtual async Task<IActionResult> FormFieldList(FormFieldSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _formModelFactory.PrepareFormFieldListModelAsync(searchModel);
            return Json(model);
        }

        public virtual async Task<IActionResult> FormFieldAddUpdate(FormFieldModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return Unauthorized();

            var entity = await _formService.GetFormByIdAsync(model.FormId);
            if (entity == null)
                return BadRequest();

            var formField = await _formFieldService.GetFormFieldByIdAsync(model.Id);

            model = await _formModelFactory.PrepareFormFieldModelAsync(model, formField);
            var view = await this.RenderPartialViewToStringAsync("_CreateOrUpdate.FormFieldAddUpdate", model);

            return Ok(new ApiResponseModel(success: true, model: new
            {
                html = view
            }));
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public virtual async Task<IActionResult> FormFieldAddUpdate(FormFieldModel model, IFormCollection form)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageForms))
                return AccessDeniedView();

            try
            {
                var entity = model.ToEntity<FormField>();
                await _formFieldService.InsertUpdateFormFieldAsync(entity);

                var formFieldOptions = await _formFieldOptionService.GetAllFormFieldOptionsAsync(formFieldId: entity.Id);
                var insertFormFieldOptions = new List<FormFieldOption>();
                var updateFormFieldOptions = new List<FormFieldOption>();
                foreach (var op in model.SelectedOptions.ToHashSet())
                {
                    if (string.IsNullOrEmpty(op))
                    {
                        continue;
                    }

                    var formFieldOption = formFieldOptions.FirstOrDefault(x => x.Name.Equals(op, StringComparison.InvariantCultureIgnoreCase));
                    if (formFieldOption is not null)
                    {
                        formFieldOptions.Remove(formFieldOption);
                    }
                    else
                    {
                        formFieldOption = new FormFieldOption()
                        {
                            FormFieldId = entity.Id
                        };
                    }

                    formFieldOption.Name = op;
                    formFieldOption.DisplayOrder = model.SelectedOptions.IndexOf(op);

                    if (formFieldOption.Id > 0)
                    {
                        updateFormFieldOptions.Add(formFieldOption);
                    }
                    else
                    {
                        insertFormFieldOptions.Add(formFieldOption);
                    }
                }

                await _formFieldOptionService.InsertFormFieldOptionAsync(insertFormFieldOptions);
                await _formFieldOptionService.UpdateFormFieldOptionAsync(updateFormFieldOptions);
                await _formFieldOptionService.DeleteFormFieldOptionAsync(formFieldOptions);

                return Ok(new ApiResponseModel(success: true, message: await _localizationService.GetResourceAsync("Admin.Common.Added")));
            }
            catch (Exception exc)
            {
                await _logger.ErrorAsync(exc.Message, exc);
                return Ok(new ApiResponseModel(success: false, message: exc.Message));
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> FormFieldDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageStudents))
                return await AccessDeniedDataTablesJson();

            var entity = await _formFieldService.GetFormFieldByIdAsync(id)
                ?? throw new ArgumentException("No record found with the specified id");

            await _formFieldService.DeleteFormFieldAsync(entity);

            return new NullJsonResult();
        }

        #endregion

        #endregion
    }
}
