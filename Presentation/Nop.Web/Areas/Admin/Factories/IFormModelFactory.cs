﻿using Nop.Core.Domain.Forms;
using Nop.Web.Areas.Admin.Models.Forms;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IFormModelFactory
    {
        #region Form

        Task<FormSearchModel> PrepareFormSearchModelAsync(FormSearchModel searchModel);
        Task<FormListModel> PrepareFormListModelAsync(FormSearchModel searchModel);
        Task<FormModel> PrepareFormModelAsync(
            FormModel model,
            Form entity
            );

        #endregion

        #region Form Field

        Task<FormFieldSearchModel> PrepareFormFieldSearchModelAsync(FormFieldSearchModel searchModel);
        Task<FormFieldListModel> PrepareFormFieldListModelAsync(FormFieldSearchModel searchModel);
        Task<FormFieldModel> PrepareFormFieldModelAsync(
                    FormFieldModel model,
                    FormField entity
                    );
        #endregion

    }
}
