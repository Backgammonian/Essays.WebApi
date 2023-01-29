using System.ComponentModel.DataAnnotations;

namespace Essays.WebApi.Models
{
    public class Country
    {
        [Key]
        public string CountryAbbreviation { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public ICollection<CountriesOfAuthors>? CountriesOfAuthors { get; set; }
    }
}
