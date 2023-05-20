using ResellHub.Enums;

namespace ResellHub.Entities
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User RoleOwner { get; set; }
        public UserRoles UserRole { get; set; } = UserRoles.User;
    }
}
