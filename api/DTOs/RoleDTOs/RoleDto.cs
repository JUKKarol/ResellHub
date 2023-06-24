using ResellHub.Enums;

namespace ResellHub.DTOs.RoleDTOs
{
    public class RoleDto
    {
        public Guid UserId { get; set; }
        public UserRoles UserRole { get; set; } = UserRoles.User;
    }
}
