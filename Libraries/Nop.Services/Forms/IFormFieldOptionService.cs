using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Forms;
using Nop.Core;

namespace Nop.Services.Forms
{
    public partial interface IFormFieldOptionService
    {
        Task<IPagedList<FormFieldOption>> GetAllFormFieldOptionsAsync(
            int formFieldId = default,
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<FormFieldOption> GetFormFieldOptionByIdAsync(int id);
        Task InsertFormFieldOptionAsync(FormFieldOption entity);
        Task InsertFormFieldOptionAsync(IList<FormFieldOption> entities);
        Task UpdateFormFieldOptionAsync(FormFieldOption entity);
        Task UpdateFormFieldOptionAsync(IList<FormFieldOption> entities);
        Task DeleteFormFieldOptionAsync(FormFieldOption entity);
        Task DeleteFormFieldOptionAsync(IList<FormFieldOption> entities);
    }
}
