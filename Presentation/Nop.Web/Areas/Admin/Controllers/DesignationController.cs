using Microsoft.AspNetCore.Mvc;

using Nop.Core.Domain.Utilities;
using Nop.Core.Domain.Staffs;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Staffs;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Staffs;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class DesignationController : BaseAdminController
    {
        #region Fields

        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IDesignationModelFactory _designationModelFactory;
        protected readonly IDesignationService _designationService;
        protected readonly Services.Logging.ILogger _logger;

        #endregion

        #region Ctor

        public DesignationController(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IDesignationModelFactory designationModelFactory,
            IDesignationService designationService,
            Services.Logging.ILogger logger
            )
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _designationModelFactory = designationModelFactory;
            _designationService = designationService;
            _logger = logger;
        }

        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDesignations))
                return AccessDeniedView();

            // prepare model
            var model = await _designationModelFactory.PrepareDesignationSearchModelAsync(new DesignationSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(DesignationSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDesignations))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _designationModelFactory.PrepareDesignationListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DesignationAdd(DesignationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDesignations))
                return AccessDeniedView();

            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException(nameof(model.Name));

                var entity = model.ToEntity<Designation>();
                await _designationService.InsertDesignationAsync(entity);

                return Ok(new ApiResponseModel(success: true, message: await _localizationService.GetResourceAsync("Admin.Common.Added")));
            }
            catch (Exception exc)
            {
                await _logger.ErrorAsync(exc.Message, exc);
                return Ok(new ApiResponseModel(success: false, message: exc.Message));
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> DesignationUpdate(DesignationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDesignations))
                return AccessDeniedView();

            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException(nameof(model.Name));

                var entity = await _designationService.GetDesignationByIdAsync(model.Id)
                    ?? throw new ArgumentException("No Designation Found with this Id ");

                entity = model.ToEntity(entity);
                await _designationService.UpdateDesignationAsync(entity);

                return new NullJsonResult();
            }
            catch (Exception exc)
            {
                await _logger.ErrorAsync(exc.Message, exc);
                return Ok(new ApiResponseModel(success: false, message: exc.Message));
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> DesignationDelete(DesignationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDesignations))
                return AccessDeniedView();

            var entity = await _designationService.GetDesignationByIdAsync(model.Id)
                     ?? throw new ArgumentException("No Designation Found with this Id ");

            await _designationService.DeleteDesignationAsync(entity);

            return Json(new ApiResponseModel(success: true, message: await _localizationService.GetResourceAsync("admin.common.delete")));
        }
    }
}
