using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDriveX.Domain.BaseEntities
{
    public abstract class BaseEntity : BaseEntityGeneric<Guid>
    {
        protected BaseEntity() 
        { 
            this.Id = Guid.NewGuid();
        }

    }
}
