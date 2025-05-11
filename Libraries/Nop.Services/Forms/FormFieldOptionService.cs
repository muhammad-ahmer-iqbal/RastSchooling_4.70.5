using Nop.Core;
using Nop.Data;
using Nop.Core.Domain.Forms;

namespace Nop.Services.Forms
{
    public partial class FormFieldOptionService : IFormFieldOptionService
    {
        #region Fields

        protected readonly IRepository<FormFieldOption> _formfieldoptionRepository;

        #endregion

        #region Ctor

        public FormFieldOptionService(IRepository<FormFieldOption> formfieldoptionRepository)
        {
            _formfieldoptionRepository = formfieldoptionRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<FormFieldOption>> GetAllFormFieldOptionsAsync(
            int formFieldId = default,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _formfieldoptionRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.FormFieldId.Equals(formFieldId));

                query = query.OrderBy(x => x.DisplayOrder);

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<FormFieldOption> GetFormFieldOptionByIdAsync(int id)
        {
            return await _formfieldoptionRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertFormFieldOptionAsync(FormFieldOption entity)
        {
            await _formfieldoptionRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateFormFieldOptionAsync(FormFieldOption entity)
        {
            await _formfieldoptionRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteFormFieldOptionAsync(FormFieldOption entity)
        {
            await _formfieldoptionRepository.DeleteAsync(entity);
        }

        public virtual async Task InsertUpdateFormFieldOptionAsync(FormFieldOption entity)
        {
            await _formfieldoptionRepository.InsertUpdateAsync(entity);
        }

        #endregion
    }
}
