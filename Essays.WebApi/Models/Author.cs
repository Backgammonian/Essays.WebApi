namespace Essays.WebApi.Models
{
    public class Author
    {
        public string AuthorId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Essay> Essays { get; set; }
        public ICollection<CountriesOfAuthors> CountriesOfAuthors { get; set; }
    }
}
