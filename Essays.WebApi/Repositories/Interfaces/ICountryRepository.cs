﻿using Essays.WebApi.DTOs;
using Essays.WebApi.Models;

namespace Essays.WebApi.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<ICollection<Country>> GetCountries(int pageNumber, int pageSize);
        Task<Country?> GetCountry(string countryAbbreviation);
        Task<Country?> GetCountryTracking(string countryAbbreviation);
        Task<ICollection<Author>?> GetAuthorsFromCountry(string countryAbbreviation);
        Task<bool> DoesCountryExist(string countryAbbreviation);
        Task<bool> CreateCountry(CreateCountryDto createCountryDto);
        Task<bool> UpdateCountry(Country country);
        Task<bool> DeleteCountry(Country country);
        Task<bool> Save();
    }
}
