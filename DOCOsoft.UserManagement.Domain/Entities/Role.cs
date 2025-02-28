using System;
using System.Collections.Generic;
using DOCOsoft.UserManagement.Domain.Common;

namespace DOCOsoft.UserManagement.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }
        public ICollection<User> Users { get; private set; } = new List<User>();

        private Role() { } 

        public Role(Guid id, string name) : base(id) 
        {
            Name = name;
        }
    }
}
