using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Forms
{
    public partial class Form : BaseEntity, ISlugSupported
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int DepartmentId { get; set; }
    }
}
