using Essays.WebApi.Data;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Repositories.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _dataContext.Countries
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Country>> GetCountries(int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _dataContext.Countries
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Country?> GetCountry(string countryAbbreviation)
        {
            return await _dataContext.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CountryAbbreviation == countryAbbreviation);
        }

        public async Task<ICollection<Author>?> GetAuthorsFromCountry(string countryAbbreviation)
        {
            var any = await DoesCountryExist(countryAbbreviation);
            if (!any)
            {
                return null;
            }

            var authorsFromCountry = await _dataContext.CountriesOfAuthors
                .AsNoTracking()
                .Where(x => x.CountryAbbreviation == countryAbbreviation)
                .ToListAsync();

            var authors = new List<Author>();
            foreach (var authorFromCountry in authorsFromCountry)
            {
                var author = await _dataContext.Authors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AuthorId == authorFromCountry.AuthorId);

                if (author != null)
                {
                    authors.Add(author);
                }
            }

            return authors;
        }

        public async Task<bool> DoesCountryExist(string countryAbbreviation)
        {
            return await _dataContext.Countries
                .AnyAsync(x => x.CountryAbbreviation == countryAbbreviation);
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await _dataContext.Countries.AddAsync(country);
            return await Save();
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            _dataContext.Countries.Update(country);
            return await Save();
        }

        public async Task<bool> DeleteCountry(Country country)
        {
            _dataContext.Countries.Remove(country);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
