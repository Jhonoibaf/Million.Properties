using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Properties.Domain.Common;

public class BaseEntityModel
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
}
