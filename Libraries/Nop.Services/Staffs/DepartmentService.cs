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
    public partial class DepartmentService : IDepartmentService
    {
        #region Fields

        protected readonly IRepository<Department> _departmentRepository;

        #endregion

        #region Ctor

        public DepartmentService(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<Department>> GetAllDepartmentsAsync(
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _departmentRepository.GetAllPagedAsync(query =>
            {
                return query;
            }, pageIndex, pageSize, includeDeleted: false);
        }
        public virtual async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertDepartmentAsync(Department entity)
        {
            await _departmentRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateDepartmentAsync(Department entity)
        {
            await _departmentRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteDepartmentAsync(Department entity)
        {
            await _departmentRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
