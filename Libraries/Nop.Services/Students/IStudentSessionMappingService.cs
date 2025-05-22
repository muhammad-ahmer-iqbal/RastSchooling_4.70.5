using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Students;
using Nop.Core;

namespace Nop.Services.Students
{
    public partial interface IStudentSessionMappingService
    {
        Task<IPagedList<StudentSessionMapping>> GetAllStudentSessionMappingsAsync(
           int customerId,
           int pageIndex = default,
           int pageSize = int.MaxValue
           );
        Task<StudentSessionMapping> GetStudentSessionMappingByIdAsync(int id);
        Task InsertStudentSessionMappingAsync(StudentSessionMapping entity);
        Task InsertStudentSessionMappingAsync(IEnumerable<StudentSessionMapping> entities);
        Task UpdateStudentSessionMappingAsync(StudentSessionMapping entity);
        Task UpdateStudentSessionMappingAsync(IEnumerable<StudentSessionMapping> entities);
        Task DeleteStudentSessionMappingAsync(StudentSessionMapping entity);
        Task DeleteStudentSessionMappingAsync(IEnumerable<StudentSessionMapping> entities);
    }
}
