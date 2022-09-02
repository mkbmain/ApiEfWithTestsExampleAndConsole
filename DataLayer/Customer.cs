using System;
using System.Collections.Generic;

namespace DataLayer
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = null!;

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
