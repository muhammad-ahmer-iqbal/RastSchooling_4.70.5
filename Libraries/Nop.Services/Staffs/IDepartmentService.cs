using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Staffs;
using Nop.Core;

namespace Nop.Services.Staffs
{
    public partial interface IDepartmentService
    {
        Task<IPagedList<Department>> GetAllDepartmentsAsync(
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<Department> GetDepartmentByIdAsync(int id);
        Task InsertDepartmentAsync(Department entity);
        Task UpdateDepartmentAsync(Department entity);
        Task DeleteDepartmentAsync(Department entity);
    }
}
