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
    public partial class StudentSessionMappingService : IStudentSessionMappingService
    {
        #region Fields

        protected readonly IRepository<StudentSessionMapping> _studentSessionMappingRepository;

        #endregion

        #region Ctor

        public StudentSessionMappingService(IRepository<StudentSessionMapping> studentSessionMappingRepository)
        {
            _studentSessionMappingRepository = studentSessionMappingRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<StudentSessionMapping>> GetAllStudentSessionMappingsAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _studentSessionMappingRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.CustomerId.Equals(customerId));

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<StudentSessionMapping> GetStudentSessionMappingByIdAsync(int id)
        {
            return await _studentSessionMappingRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertStudentSessionMappingAsync(StudentSessionMapping entity)
        {
            await _studentSessionMappingRepository.InsertAsync(entity);
        }
        public virtual async Task InsertStudentSessionMappingAsync(IEnumerable<StudentSessionMapping> entities)
        {
            await _studentSessionMappingRepository.InsertAsync([.. entities]);
        }
        public virtual async Task UpdateStudentSessionMappingAsync(StudentSessionMapping entity)
        {
            await _studentSessionMappingRepository.UpdateAsync(entity);
        }
        public virtual async Task UpdateStudentSessionMappingAsync(IEnumerable<StudentSessionMapping> entities)
        {
            await _studentSessionMappingRepository.UpdateAsync([.. entities]);
        }
        public virtual async Task DeleteStudentSessionMappingAsync(StudentSessionMapping entity)
        {
            await _studentSessionMappingRepository.DeleteAsync(entity);
        }
        public virtual async Task DeleteStudentSessionMappingAsync(IEnumerable<StudentSessionMapping> entities)
        {
            await _studentSessionMappingRepository.DeleteAsync([.. entities]);
        }

        #endregion
    }
}
