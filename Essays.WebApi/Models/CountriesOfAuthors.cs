namespace Essays.WebApi.Models
{
    public class CountriesOfAuthors
    {
        public string? AuthorId { get; set; }
        public string? CountryAbbreviation { get; set; }
        public Author? Author { get; set; }
        public Country? Country { get; set; }
    }
}
