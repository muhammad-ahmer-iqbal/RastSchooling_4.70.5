using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Students;

namespace Nop.Services.Students
{
    public partial interface IStudentExtensionService
    {
        Task<IPagedList<StudentExtension>> GetAllStudentExtensionsAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<StudentExtension> GetStudentExtensionByIdAsync(int id);
        Task InsertStudentExtensionAsync(StudentExtension entity);
        Task UpdateStudentExtensionAsync(StudentExtension entity);
        Task DeleteStudentExtensionAsync(StudentExtension entity);
        Task<StudentExtension> GetStudentExtensionByCustomerIdAsync(int customerId);
    }
}
