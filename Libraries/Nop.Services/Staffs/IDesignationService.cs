using Nop.Core.Domain.Staffs;
using Nop.Core;

namespace Nop.Services.Staffs
{
    public partial interface IDesignationService
    {
        Task<IPagedList<Designation>> GetAllDesignationsAsync(
                    int pageIndex = default,
                    int pageSize = int.MaxValue
                    );
        Task<Designation> GetDesignationByIdAsync(int id);
        Task InsertDesignationAsync(Designation entity);
        Task UpdateDesignationAsync(Designation entity);
        Task DeleteDesignationAsync(Designation entity);
    }
}
