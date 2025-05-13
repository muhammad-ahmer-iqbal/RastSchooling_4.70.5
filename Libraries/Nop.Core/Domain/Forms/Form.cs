using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.Forms
{
    public partial class Form : BaseEntity
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
