using Nop.Core;
using Nop.Core.Domain.Staffs;

namespace Nop.Services.Staffs
{
    public partial interface ITeacherExtensionService
    {
        Task<IPagedList<TeacherExtension>> GetAllTeacherExtensionsAsync(
            int customerId,
            int pageIndex = default,
            int pageSize = int.MaxValue
            );
        Task<TeacherExtension> GetTeacherExtensionByIdAsync(int id);
        Task InsertTeacherExtensionAsync(TeacherExtension entity);
        Task UpdateTeacherExtensionAsync(TeacherExtension entity);
        Task DeleteTeacherExtensionAsync(TeacherExtension entity);
        Task<TeacherExtension> GetTeacherExtensionByCustomerIdAsync(int customerId);
        Task InsertUpdateTeacherExtensionAsync(TeacherExtension entity);
    }
}
