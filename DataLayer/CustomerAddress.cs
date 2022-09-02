using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class CustomerAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PostCode { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNumber { get; set; } 
        public string BuildingName { get; set; }
        public string City { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
    }
}
