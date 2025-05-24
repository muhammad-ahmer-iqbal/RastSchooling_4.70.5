using Nop.Core.Domain.Forms;
using Nop.Web.Models.Forms;

namespace Nop.Web.Factories
{
    public partial interface IFormModelFactory
    {
        Task<FormModel> PrepareFormModelAsync(
            FormModel model,
            Form entity
            );
    }
}
