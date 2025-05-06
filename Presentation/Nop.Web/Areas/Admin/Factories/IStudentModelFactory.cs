using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Students;
using Nop.Web.Areas.Admin.Models.Students;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IStudentModelFactory
    {
        #region Student

        Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel);
        Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel);
        Task<StudentModel> PrepareStudentModelAsync(
            StudentModel model,
            Customer entity,
            bool excludeProperties = default
            );

        #endregion

        #region Student Leave

        Task<StudentLeaveSearchModel> PrepareStudentLeaveSearchModelAsync(StudentLeaveSearchModel searchModel);
        Task<StudentLeaveListModel> PrepareStudentLeaveListModelAsync(StudentLeaveSearchModel searchModel);

        #endregion
    }
}
