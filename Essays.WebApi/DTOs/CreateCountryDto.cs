using System.ComponentModel.DataAnnotations;

namespace Essays.WebApi.DTOs
{
    public class CreateCountryDto
    {
        [Required]
        public string CountryAbbreviation { get; set; } = string.Empty;

        [Required]
        public string CountryName { get; set; } = string.Empty;
    }
}
