using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Students
{
    public partial class StudentExtension : BaseEntity
    {
        public int CustomerId { get; set; }
        public int MonthlyFee { get; set; }
    }
}
