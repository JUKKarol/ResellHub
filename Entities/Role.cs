using ResellHub.Enums;

namespace ResellHub.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User RoleOwner { get; set; }
        public UserRoles Permissions { get; set; } = UserRoles.User;
    }
}
