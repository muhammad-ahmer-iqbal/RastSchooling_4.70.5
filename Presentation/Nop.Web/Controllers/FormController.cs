using DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Services.Forms;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.Forms;
using ILogger = Nop.Services.Logging.ILogger;

namespace Nop.Web.Controllers
{
    [CheckAccessFormSubmissionAttribute]
    [CheckAccessPublicStore(true)]
    public class FormController : BasePublicController
    {
        #region Fields

        protected readonly ILocalizationService _localizationService;
        protected readonly INotificationService _notificationService;
        protected readonly IFormService _formService;
        protected readonly IFormFieldService _formFieldService;
        protected readonly IFormFieldOptionService _formFieldOptionService;
        protected readonly ILogger _logger;
        protected readonly IFormModelFactory _formModelFactory;

        #endregion

        #region Ctor

        public FormController(
            ILocalizationService localizationService,
            INotificationService notificationService,
            IFormService formService,
            IFormFieldService formFieldService,
            IFormFieldOptionService formFieldOptionService,
            ILogger logger,
            IFormModelFactory formModelFactory
            )
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _formService = formService;
            _formFieldService = formFieldService;
            _formFieldOptionService = formFieldOptionService;
            _logger = logger;
            _formModelFactory = formModelFactory;
        }

        #endregion

        #region Methods

        public virtual async Task<IActionResult> FormDetail(int formId)
        {
            var form = await _formService.GetFormByIdAsync(formId);

            var model = await _formModelFactory.PrepareFormModelAsync(new FormModel(), form);
            return View(model);
        }

        #endregion
    }
}
