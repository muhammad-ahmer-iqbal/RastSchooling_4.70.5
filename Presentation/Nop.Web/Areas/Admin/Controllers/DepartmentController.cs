using Microsoft.AspNetCore.Mvc;

using Nop.Core.Domain.Utilities;
using Nop.Core.Domain.Staffs;
using Nop.Core.Domain.Students;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Staffs;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Staffs;
using Nop.Web.Framework.Mvc;

using System.Text.RegularExpressions;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class DepartmentController : BaseAdminController
    {
        #region Fields

        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IDepartmentModelFactory _departmentModelFactory;
        protected readonly IDepartmentService _departmentService;
        protected readonly Services.Logging.ILogger _logger;

        #endregion

        #region Ctor

        public DepartmentController(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IDepartmentModelFactory departmentModelFactory,
            IDepartmentService departmentService,
            Services.Logging.ILogger logger
            )
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _departmentModelFactory = departmentModelFactory;
            _departmentService = departmentService;
            _logger = logger;
        }

        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()

        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDepartments))
                return AccessDeniedView();

            // prepare model
            var model = await _departmentModelFactory.PrepareDepartmentSearchModelAsync(new DepartmentSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(DepartmentSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDepartments))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _departmentModelFactory.PrepareDepartmentListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DepartmentAdd(DepartmentModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDepartments))
                return AccessDeniedView();

            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException(nameof(model.Name));

                var entity = model.ToEntity<Department>();
                await _departmentService.InsertDepartmentAsync(entity);

                return Ok(new ApiResponseModel(success: true, message: await _localizationService.GetResourceAsync("Admin.Common.Added")));
            }
            catch (Exception exc)
            {
                await _logger.ErrorAsync(exc.Message, exc);
                return Ok(new ApiResponseModel(success: false, message: exc.Message));
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> DepartmentUpdate(DepartmentModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDepartments))
                return AccessDeniedView();

            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException(nameof(model.Name));

                var entity = await _departmentService.GetDepartmentByIdAsync(model.Id)
                    ?? throw new ArgumentException("No Department Found with this Id ");

                entity = model.ToEntity(entity);
                await _departmentService.UpdateDepartmentAsync(entity);

                return new NullJsonResult();
            }
            catch (Exception exc)
            {
                await _logger.ErrorAsync(exc.Message, exc);
                return Ok(new ApiResponseModel(success: false, message: exc.Message));
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> DepartmentDelete(DepartmentModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDepartments))
                return AccessDeniedView();

            var entity = await _departmentService.GetDepartmentByIdAsync(model.Id)
                     ?? throw new ArgumentException("No Department Found with this Id ");

            await _departmentService.DeleteDepartmentAsync(entity);

            return Json(new ApiResponseModel(success: true, message: await _localizationService.GetResourceAsync("admin.common.delete")));
        }
    }
}
