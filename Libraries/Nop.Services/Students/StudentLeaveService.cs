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
    public partial class StudentLeaveService : IStudentLeaveService
    {
        #region Fields

        protected readonly IRepository<StudentLeave> _studentLeaveRepository;

        #endregion

        #region Ctor

        public StudentLeaveService(IRepository<StudentLeave> studentLeaveRepository)
        {
            _studentLeaveRepository = studentLeaveRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<StudentLeave>> GetAllStudentLeavesAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _studentLeaveRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.CustomerId.Equals(customerId));

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<StudentLeave> GetStudentLeaveByIdAsync(int id)
        {
            return await _studentLeaveRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertStudentLeaveAsync(StudentLeave entity)
        {
            await _studentLeaveRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateStudentLeaveAsync(StudentLeave entity)
        {
            await _studentLeaveRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteStudentLeaveAsync(StudentLeave entity)
        {
            await _studentLeaveRepository.DeleteAsync(entity);
        }
        public virtual async Task<StudentLeave> GetStudentLeaveByCustomerIdAsync(int customerId)
        {
            return await _studentLeaveRepository.Table
                .FirstOrDefaultAsync(x => x.CustomerId.Equals(customerId));
        }

        #endregion
    }
}
