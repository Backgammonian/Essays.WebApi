namespace Essays.WebApi.Models
{
    public class CountriesOfAuthors
    {
        public string AuthorId { get; set; } = string.Empty;
        public string CountryAbbreviation { get; set; } = string.Empty;
        public Author Author { get; set; }
        public Country Country { get; set; }
    }
}
