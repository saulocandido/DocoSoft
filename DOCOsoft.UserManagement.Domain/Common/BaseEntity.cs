using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOCOsoft.UserManagement.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get;  set; }

        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; private set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void UpdateModifiedDate()
        {
            LastModifiedDate = DateTime.UtcNow;
        }

        protected BaseEntity() => Id = Guid.NewGuid();

        protected BaseEntity(Guid id) => Id = id; 
    }
}
