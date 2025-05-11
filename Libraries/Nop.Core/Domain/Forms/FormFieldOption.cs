using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Forms
{
    public partial class FormFieldOption : BaseEntity
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public int FormFieldId { get; set; }
    }
}
