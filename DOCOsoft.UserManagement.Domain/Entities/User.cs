using DOCOsoft.UserManagement.Domain.Common;
using DOCOsoft.UserManagement.Domain.Entities;
using DOCOsoft.UserManagement.Domain.ValueObjects;

public class User : BaseEntity
{
    public FullName Name { get; private set; }
    public Email Email { get; private set; }
    public ICollection<Role> Roles { get; private set; } = new List<Role>();

    private User() { }

    public User(FullName name, Email email)
    {
        Name = name;
        Email = email;
    }

    public void UpdateUser(FullName name, Email email)
    {
        Name = name;
        Email = email;
        UpdateModifiedDate();
    }

    public void AddRole(Role role)
    {
        if (role != null && !Roles.Any(r => r.Id == role.Id))
            Roles.Add(role);
    }

    public void RemoveRole(Role role)
    {
        if (role != null && Roles.Any(r => r.Id == role.Id))
            Roles.Remove(role);
    }
}
