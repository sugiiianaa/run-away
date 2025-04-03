using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunAway.Domain.Commons
{
    public class BaseEntity<T>
    {
        public T Id { get; protected set; } = default!;

        protected BaseEntity() { }  // For Entity Framework

        protected BaseEntity(T id) 
        {
            Id = id;
        }
    }
}
