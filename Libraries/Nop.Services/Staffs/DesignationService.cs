using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Staffs;
using Nop.Core;
using Nop.Data;

namespace Nop.Services.Staffs
{
    public partial class DesignationService : IDesignationService
    {
        #region Fields

        protected readonly IRepository<Designation> _designationRepository;

        #endregion

        #region Ctor

        public DesignationService(IRepository<Designation> designationRepository)
        {
            _designationRepository = designationRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<Designation>> GetAllDesignationsAsync(
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _designationRepository.GetAllPagedAsync(query =>
            {
                return query;
            }, pageIndex, pageSize, includeDeleted: false);
        }
        public virtual async Task<Designation> GetDesignationByIdAsync(int id)
        {
            return await _designationRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertDesignationAsync(Designation entity)
        {
            await _designationRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateDesignationAsync(Designation entity)
        {
            await _designationRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteDesignationAsync(Designation entity)
        {
            await _designationRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
