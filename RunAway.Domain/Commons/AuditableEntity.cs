using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunAway.Domain.Commons
{
    public class AuditableEntity<T> : BaseEntity<T>
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; private set;}
        
        protected AuditableEntity() { }

        protected AuditableEntity(T id) : base(id) { }

        public void SetUpdatedAt()
        {
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
