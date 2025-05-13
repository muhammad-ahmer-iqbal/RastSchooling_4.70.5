using Nop.Services.Forms;
using Nop.Web.Areas.Admin.Models.Forms;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Core.Domain.Forms;
using Nop.Services;
using Nop.Services.Localization;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class FormModelFactory : IFormModelFactory
    {
        #region Fields

        protected readonly IFormService _formService;
        protected readonly IFormFieldService _formFieldService;
        protected readonly IFormFieldOptionService _formFieldOptionService;
        protected readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public FormModelFactory(
            IFormService formService,
            IFormFieldService formFieldService,
            IFormFieldOptionService formFieldOptionService,
            ILocalizationService localizationService
            )
        {
            _formService = formService;
            _formFieldService = formFieldService;
            _formFieldOptionService = formFieldOptionService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        #region Form

        public virtual async Task<FormSearchModel> PrepareFormSearchModelAsync(FormSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual async Task<FormListModel> PrepareFormListModelAsync(FormSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _formService.GetAllFormsAsync(
                name: searchModel.Name,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new FormListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<FormModel>();

                    return tempModel;
                });
            });

            return model;
        }

        public virtual async Task<FormModel> PrepareFormModelAsync(
            FormModel model,
            Form entity,
            bool excludeProperties = default
            )
        {
            if (entity != null)
            {
                //fill in model values from the entity
                model ??= entity.ToModel<FormModel>();
            }
            else
            {
                if (!excludeProperties)
                {

                }
            }

            model.FormFieldSearchModel = await this.PrepareFormFieldSearchModelAsync(model.FormFieldSearchModel);

            return model;
        }

        #endregion

        #region Form Field

        public virtual async Task<FormFieldSearchModel> PrepareFormFieldSearchModelAsync(FormFieldSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual async Task<FormFieldListModel> PrepareFormFieldListModelAsync(FormFieldSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(nameof(searchModel));

            var allEntities = await _formFieldService.GetAllFormFieldsAsync(
               formId: searchModel.FormId,
               pageIndex: searchModel.Page - 1,
               pageSize: searchModel.PageSize);

            var model = await new FormFieldListModel().PrepareToGridAsync(searchModel, allEntities, () =>
            {
                return allEntities.SelectAwait(async entity =>
                {
                    var tempModel = entity.ToModel<FormFieldModel>();
                    tempModel.ControlTypeString = await _localizationService.GetLocalizedEnumAsync(tempModel.ControlType);

                    var fieldOptions = await _formFieldOptionService.GetAllFormFieldOptionsAsync(formFieldId: tempModel.Id);
                    tempModel.Options = string.Join("<br />", fieldOptions.Select(x => x.Name));

                    return tempModel;
                });
            });

            return model;
        }

        public virtual async Task<FormFieldModel> PrepareFormFieldModelAsync(
            FormFieldModel model,
            FormField entity,
            bool excludeProperties = default
            )
        {
            if (entity is not null)
            {
                //fill in model values from the entity
                if (model is null)
                {
                    model = new FormFieldModel();
                }
                model = entity.ToModel<FormFieldModel>();

                var fieldOptions = await _formFieldOptionService.GetAllFormFieldOptionsAsync(formFieldId: model.Id);
                model.FormFieldOptionModels = fieldOptions
                    .Select(x => x.ToModel<FormFieldOptionModel>())
                    .ToList();
            }
            else
            {
                if (!excludeProperties)
                {
                }
            }

            model.AvailableControlTypes = (await ControlTypeEnum.TextBox.ToSelectListAsync()).ToList();
            return model;
        }

        #endregion

        #endregion
    }
}
