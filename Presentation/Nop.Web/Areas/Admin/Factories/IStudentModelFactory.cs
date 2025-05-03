using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Students;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IStudentModelFactory
    {
        Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel);
        Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel);
        Task<StudentModel> PrepareStudentModelAsync(StudentModel model, Customer entity);
    }
}
