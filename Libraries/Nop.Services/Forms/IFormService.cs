using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Students;
using Nop.Core;
using Nop.Core.Domain.Forms;

namespace Nop.Services.Forms
{
    public partial interface IFormService
    {
        Task<IPagedList<Form>> GetAllFormsAsync(
            string name = default,
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<Form> GetFormByIdAsync(int id);
        Task InsertFormAsync(Form entity);
        Task UpdateFormAsync(Form entity);
        Task DeleteFormAsync(Form entity);
        Task InsertUpdateFormAsync(Form entity);
    }
}
