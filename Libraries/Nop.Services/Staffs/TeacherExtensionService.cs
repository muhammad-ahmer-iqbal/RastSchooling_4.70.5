using Nop.Core;
using Nop.Data;
using Nop.Core.Domain.Staffs;

namespace Nop.Services.Staffs
{
    public partial class TeacherExtensionService : ITeacherExtensionService
    {
        #region Fields

        protected readonly IRepository<TeacherExtension> _teacherExtensionRepository;

        #endregion

        #region Ctor

        public TeacherExtensionService(IRepository<TeacherExtension> teacherExtensionRepository)
        {
            _teacherExtensionRepository = teacherExtensionRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<TeacherExtension>> GetAllTeacherExtensionsAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            )
        {
            return await _teacherExtensionRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => x.CustomerId == customerId);

                return query;
            }, pageIndex, pageSize);
        }
        public virtual async Task<TeacherExtension> GetTeacherExtensionByIdAsync(int id)
        {
            return await _teacherExtensionRepository.GetByIdAsync(id);
        }
        public virtual async Task InsertTeacherExtensionAsync(TeacherExtension entity)
        {
            await _teacherExtensionRepository.InsertAsync(entity);
        }
        public virtual async Task UpdateTeacherExtensionAsync(TeacherExtension entity)
        {
            await _teacherExtensionRepository.UpdateAsync(entity);
        }
        public virtual async Task DeleteTeacherExtensionAsync(TeacherExtension entity)
        {
            await _teacherExtensionRepository.DeleteAsync(entity);
        }
        public virtual async Task<TeacherExtension> GetTeacherExtensionByCustomerIdAsync(int customerId)
        {
            return await _teacherExtensionRepository.Table
                .FirstOrDefaultAsync(x => x.CustomerId.Equals(customerId));
        }
        public virtual async Task InsertUpdateTeacherExtensionAsync(TeacherExtension entity)
        {
            await _teacherExtensionRepository.InsertUpdateAsync(entity);
        }

        #endregion
    }
}
