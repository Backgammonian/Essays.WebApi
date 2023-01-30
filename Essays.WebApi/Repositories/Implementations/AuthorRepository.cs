using Essays.WebApi.Data;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Repositories.Implementations
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _dataContext;

        public AuthorRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Author>> GetAuthors()
        {
            return await _dataContext.Authors
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Author?> GetAuthor(string authorId)
        {
            return await _dataContext.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);
        }

        public async Task<ICollection<Essay>?> GetEssaysOfAuthor(string authorId)
        {
            var any = await DoesAuthorExist(authorId);
            if (!any)
            {
                return null;
            }

            return await _dataContext.Essays
                .AsNoTracking()
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<ICollection<Country>?> GetCountriesOfAuthor(string authorId)
        {
            var any = await DoesAuthorExist(authorId);
            if (!any)
            {
                return null;
            }

            var countriesOfAuthor = await _dataContext.CountriesOfAuthors
                .AsNoTracking()
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();

            var countries = new List<Country>();
            foreach (var countryOfAuthor in countriesOfAuthor)
            {
                var country = await _dataContext.Countries
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CountryAbbreviation == countryOfAuthor.CountryAbbreviation);

                if (country != null)
                {
                    countries.Add(country);
                }
            }

            return countries;
        }

        public async Task<bool> DoesAuthorExist(string authorId)
        {
            return await _dataContext.Authors
                .AnyAsync(a => a.AuthorId == authorId);
        }

        public async Task<bool> CreateAuthor(Author author)
        {
            await _dataContext.Authors.AddAsync(author);
            return await Save();
        }

        public async Task<bool> AddCountryOfAuthor(string authorId, string countryAbbreviation)
        {
            var countryOfAuthor = new CountriesOfAuthors()
            {
                AuthorId = authorId,
                CountryAbbreviation = countryAbbreviation
            };

            await _dataContext.CountriesOfAuthors.AddAsync(countryOfAuthor);
            return await Save();
        }

        public async Task<bool> RemoveCountryOfAuthor(string authorId, string countryAbbreviation)
        {
            var countryOfAuthor = await _dataContext.CountriesOfAuthors
                .FindAsync(authorId, countryAbbreviation);

            if (countryOfAuthor == null)
            {
                return false;
            }

            _dataContext.CountriesOfAuthors.Remove(countryOfAuthor);
            return await Save();
        }

        public async Task<bool> UpdateAuthor(Author author)
        {
            _dataContext.Authors.Update(author);
            return await Save();
        }

        public async Task<bool> DeleteAuthor(Author author)
        {
            _dataContext.Authors.Remove(author);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
