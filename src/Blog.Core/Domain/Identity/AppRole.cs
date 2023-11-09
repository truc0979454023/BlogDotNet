using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Domain.Identity
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole<Guid> // kế thừa lại class của identityUser vời kiểu khóa chính là guid
    {
        [Required]
        [MaxLength(200)]
        public required string DisplayName { get; set; }
    }
}