using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Forms
{
    public partial class FormField : BaseEntity
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public int FormId { get; set; }
        public bool Required { get; set; }
        public int ControlTypeId { get; set; }
        public ControlTypeEnum ControlType
        {
            get => (ControlTypeEnum)this.ControlTypeId;
            set { this.ControlTypeId = (int)value; }
        }
    }
}
