using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Students;
using Nop.Core;
using Nop.Data;

namespace Nop.Services.Students
{
    public partial class StudentExtensionService : IStudentExtensionService
    {
        #region Fields

        protected readonly IRepository<StudentExtension> _studentExtensionRepository;

        #endregion

        #region Ctor

        public StudentExtensionService(IRepository<StudentExtension> studentExtensionRepository)
        {
            _studentExtensionRepository = studentExtensionRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<StudentExtension>> GetAllStudentExtensionsAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _studentExtensionRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.CustomerId == customerId);

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<StudentExtension> GetStudentExtensionByIdAsync(int id)
        {
            return await _studentExtensionRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertStudentExtensionAsync(StudentExtension entity)
        {
            await _studentExtensionRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateStudentExtensionAsync(StudentExtension entity)
        {
            await _studentExtensionRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteStudentExtensionAsync(StudentExtension entity)
        {
            await _studentExtensionRepository.DeleteAsync(entity);
        }
        public virtual async Task<StudentExtension> GetStudentExtensionByCustomerIdAsync(int customerId)
        {
            return await _studentExtensionRepository.Table
                .FirstOrDefaultAsync(x => x.CustomerId.Equals(customerId));
        }

        #endregion
    }
}
