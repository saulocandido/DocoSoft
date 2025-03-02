namespace DOCOsoft.UserManagement.Domain.Interfaces
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
