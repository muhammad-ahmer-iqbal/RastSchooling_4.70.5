using Nop.Core.Domain.Forms;
using Nop.Services.Forms;
using Nop.Web.Models.Forms;
using StackExchange.Redis;

namespace Nop.Web.Factories
{
    public partial class FormModelFactory : IFormModelFactory
    {
        #region Fields

        protected readonly IFormService _formService;
        protected readonly IFormFieldService _formFieldService;
        protected readonly IFormFieldOptionService _formFieldOptionService;

        #endregion

        #region Ctor

        public FormModelFactory(
            IFormService formService,
            IFormFieldService formFieldService,
            IFormFieldOptionService formFieldOptionService
            )
        {
            _formService = formService;
            _formFieldService = formFieldService;
            _formFieldOptionService = formFieldOptionService;
        }

        #endregion

        #region Methods

        public virtual async Task<FormModel> PrepareFormModelAsync(
            FormModel model,
            Form entity
            )
        {
            ArgumentNullException.ThrowIfNull(nameof(entity));
            model ??= new FormModel();
            //prepare form
            model.Id = entity.Id;
            model.Name = entity.Name;

            var formFields = await _formFieldService.GetAllFormFieldsAsync(formId: entity.Id);
            model.FormFields = await Task.WhenAll(formFields
                .Select(async field =>
                {
                    var formFieldOptions = await _formFieldOptionService.GetAllFormFieldOptionsAsync(formFieldId: field.Id);
                    return new FormFieldModel()
                    {
                        Id = field.Id,
                        Name = field.Name,
                        ControlType = field.ControlType,
                        Required = field.Required,
                        Options = formFieldOptions
                            .Select(option => new FormFieldOptionModel()
                            {
                                Id = option.Id,
                                Name = option.Name,
                            })
                            .ToList()
                    };
                })
                .ToList());
            return model;
        }

        #endregion
    }
}
