using Nop.Core.Domain.Forms;
using Nop.Core;

namespace Nop.Services.Forms
{
    public partial interface IFormFieldService
    {
        Task<IPagedList<FormField>> GetAllFormFieldsAsync(
            int formId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<FormField> GetFormFieldByIdAsync(int id);
        Task InsertFormFieldAsync(FormField entity);
        Task UpdateFormFieldAsync(FormField entity);
        Task DeleteFormFieldAsync(FormField entity);
        Task InsertUpdateFormFieldAsync(FormField entity);
    }
}
