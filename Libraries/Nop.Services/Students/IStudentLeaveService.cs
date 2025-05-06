using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Students;
using Nop.Core;

namespace Nop.Services.Students
{
   public partial interface IStudentLeaveService
    {
        Task<IPagedList<StudentLeave>> GetAllStudentLeavesAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<StudentLeave> GetStudentLeaveByIdAsync(int id);
        Task InsertStudentLeaveAsync(StudentLeave entity);
        Task UpdateStudentLeaveAsync(StudentLeave entity);
        Task DeleteStudentLeaveAsync(StudentLeave entity);
    }
}
