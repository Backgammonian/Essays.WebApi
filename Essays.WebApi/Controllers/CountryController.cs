using AutoMapper;
using Essays.WebApi.DTOs;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Essays.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CountryController(IMapper mapper,
            ICountryRepository countryRepository)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(ICollection<Country>))]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryRepository.GetCountries();
            var countriesDto = _mapper.Map<List<CountryDto>>(countries);

            return Ok(countriesDto);
        }

        [HttpGet("GetCountry")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountry([FromQuery] string countryAbbreviation)
        {
            var any = await _countryRepository.DoesCountryExist(countryAbbreviation);
            if (!any)
            {
                return NotFound("Such country doesn't exist");
            }

            var country = await _countryRepository.GetCountry(countryAbbreviation);
            var countryDto = _mapper.Map<CountryDto>(country);

            return Ok(countryDto);
        }

        [HttpGet("GetAuthorsFromCountry")]
        [ProducesResponseType(200, Type = typeof(ICollection<Author>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAuthorsFromCountry([FromQuery] string countryAbbreviation)
        {
            var authors = await _countryRepository.GetAuthorsFromCountry(countryAbbreviation);
            if (authors == null)
            {
                return NotFound($"There are no authors from country with abbreviation '{countryAbbreviation}'");
            }

            return Ok(authors);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest("Country model is null!");
            }

            var countries = await _countryRepository.GetCountries();
            var existingCountry = countries
                .Where(c => c.CountryName.Trim().ToLower() == countryCreate.CountryName.Trim().ToLower())
                .FirstOrDefault();

            if (existingCountry != null)
            {
                return StatusCode(422, $"Country with name '{countryCreate.CountryName}' already exists");
            }

            var country = _mapper.Map<Country>(countryCreate);
            country.CountryAbbreviation = country.CountryAbbreviation.Trim().ToLower();
            country.CountryName = country.CountryName.Trim();

            var created = await _countryRepository.CreateCountry(country);
            if (!created)
            {
                return StatusCode(500, "Failed to create a new country");
            }

            return Ok(country.CountryAbbreviation);
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCountry([FromBody] CountryDto countryUpdate)
        {
            if (countryUpdate == null)
            {
                return BadRequest("Country model is null!");
            }

            var any = await _countryRepository.DoesCountryExist(countryUpdate.CountryAbbreviation);
            if (!any)
            {
                return NotFound("Such country doesn't exist");
            }

            var country = _mapper.Map<Country>(countryUpdate);
            country.CountryAbbreviation = country.CountryAbbreviation.Trim().ToLower();
            country.CountryName = country.CountryName.Trim();

            var updated = await _countryRepository.UpdateCountry(country);
            if (!updated)
            {
                return StatusCode(500, $"Failed to update the country with abbreviation '{country.CountryAbbreviation}'");
            }

            return Ok(country.CountryAbbreviation);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry([FromQuery] string countryAbbreviation)
        {
            var any = await _countryRepository.DoesCountryExist(countryAbbreviation);
            if (!any)
            {
                return NotFound("Such country doesn't exist");
            }

            var countryToDelete = await _countryRepository.GetCountry(countryAbbreviation);
            if (countryToDelete == null)
            {
                return NotFound("Such country doesn't exist");
            }

            var deleted = await _countryRepository.DeleteCountry(countryToDelete);
            if (!deleted)
            {
                return StatusCode(500, $"Failed to delete the country with abbreviation '{countryAbbreviation}'");
            }

            return Ok(countryToDelete.CountryAbbreviation);
        }
    }
}
