﻿using AutoMapper;
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

        [HttpGet("GetCountries")]
        [ProducesResponseType(200, Type = typeof(ICollection<CountryDto>))]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryRepository.GetCountries();
            var countriesDto = _mapper.Map<ICollection<CountryDto>>(countries);

            return Ok(countriesDto);
        }

        [HttpGet("GetCountriesFromPage")]
        [ProducesResponseType(200, Type = typeof(ICollection<CountryDto>))]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetCountries([FromQuery] int pageNumber, int pageSize)
        {
            if (pageNumber < 1 ||
                pageSize < 1)
            {
                return StatusCode(422, $"Wrong page '{pageNumber}' of size '{pageSize}'");
            }

            var countries = await _countryRepository.GetCountries(pageNumber, pageSize);
            var countriesDto = _mapper.Map<ICollection<CountryDto>>(countries);

            return Ok(countriesDto);
        }

        [HttpGet("GetCountry")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
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
        [ProducesResponseType(200, Type = typeof(ICollection<AuthorDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAuthorsFromCountry([FromQuery] string countryAbbreviation)
        {
            var authors = await _countryRepository.GetAuthorsFromCountry(countryAbbreviation);
            if (authors == null)
            {
                return NotFound($"There are no authors from country with abbreviation '{countryAbbreviation}'");
            }

            var authorsDto = _mapper.Map<ICollection<AuthorDto>>(authors);

            return Ok(authorsDto);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest();
            }

            var countries = await _countryRepository.GetCountries();
            var existingCountry = countries
                .Where(c => c.CountryName.ToLower() == countryCreate.CountryName.ToLower())
                .FirstOrDefault();

            if (existingCountry != null)
            {
                return StatusCode(422, $"Country with name '{countryCreate.CountryName}' already exists");
            }

            if (await _countryRepository.CreateCountry(countryCreate))
            {
                return Ok(countryCreate.CountryAbbreviation);
            }

            return BadRequest("Failed to create a new country");
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

            var country = await _countryRepository.GetCountryTracking(countryUpdate.CountryAbbreviation);
            if (country == null)
            {
                return NotFound("Such country doesn't exist");
            }

            country.CountryAbbreviation = countryUpdate.CountryAbbreviation.ToLower();
            country.CountryName = countryUpdate.CountryName;

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
            var countryToDelete = await _countryRepository.GetCountryTracking(countryAbbreviation);
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
