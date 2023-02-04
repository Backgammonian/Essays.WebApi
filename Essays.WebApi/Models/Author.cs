using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Models
{
    [PrimaryKey(nameof(AuthorId), nameof(Login))]
    public class Author
    {
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Essay>? Essays { get; set; }
        public ICollection<CountriesOfAuthors>? CountriesOfAuthors { get; set; }
    }
}