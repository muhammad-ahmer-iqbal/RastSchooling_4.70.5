using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Students
{
    public partial class StudentSessionMapping : BaseEntity
    {
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
    }
}
