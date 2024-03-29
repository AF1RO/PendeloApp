using Microsoft.AspNetCore.Identity;

namespace PendeloApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? CustomName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? About { get; set; }
        public string? Birthday { get; set; }
        public DateTime Created { get; set; }
    }
}