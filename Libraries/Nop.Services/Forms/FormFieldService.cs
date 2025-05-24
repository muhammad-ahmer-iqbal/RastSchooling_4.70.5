using Nop.Core;
using Nop.Data;
using Nop.Core.Domain.Forms;

namespace Nop.Services.Forms
{
    public partial class FormFieldService : IFormFieldService
    {
        #region Fields

        protected readonly IRepository<FormField> _formfieldRepository;

        #endregion

        #region Ctor

        public FormFieldService(IRepository<FormField> formfieldRepository)
        {
            _formfieldRepository = formfieldRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<FormField>> GetAllFormFieldsAsync(
            int formId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _formfieldRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.FormId.Equals(formId));

                query = query.OrderBy(x => x.DisplayOrder);

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<FormField> GetFormFieldByIdAsync(int id)
        {
            return await _formfieldRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertFormFieldAsync(FormField entity)
        {
            await _formfieldRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateFormFieldAsync(FormField entity)
        {
            await _formfieldRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteFormFieldAsync(FormField entity)
        {
            await _formfieldRepository.DeleteAsync(entity);
        }

        public virtual async Task InsertUpdateFormFieldAsync(FormField entity)
        {
            await _formfieldRepository.InsertUpdateAsync(entity);
        }

        #endregion
    }
}
