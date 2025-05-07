using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.Staffs
{
    public partial class Designation : BaseEntity, ISoftDeletedEntity
    {
        public string Name { get; set; }
        public bool Deleted { get; set; }
    }
}
