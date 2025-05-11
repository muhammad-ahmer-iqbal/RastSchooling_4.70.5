using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Students;
using Nop.Core;
using Nop.Data;
using Nop.Core.Domain.Forms;

namespace Nop.Services.Forms
{
    public partial class FormService : IFormService
    {
        #region Fields

        protected readonly IRepository<Form> _formRepository;

        #endregion

        #region Ctor

        public FormService(IRepository<Form> formRepository)
        {
            _formRepository = formRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<Form>> GetAllFormsAsync(
            string name = default,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _formRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<Form> GetFormByIdAsync(int id)
        {
            return await _formRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertFormAsync(Form entity)
        {
            await _formRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateFormAsync(Form entity)
        {
            await _formRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteFormAsync(Form entity)
        {
            await _formRepository.DeleteAsync(entity);
        }

        public virtual async Task InsertUpdateFormAsync(Form entity)
        {
            await _formRepository.InsertUpdateAsync(entity);
        }

        #endregion
    }
}
