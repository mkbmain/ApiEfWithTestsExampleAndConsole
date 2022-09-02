using System;
using System.Collections.Generic;

namespace DataLayer
{
    public  class Order
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Created { get; set; }
        public decimal Total { get; set; }

        public virtual Customer Customer { get; set; } = null!;
    }
}
