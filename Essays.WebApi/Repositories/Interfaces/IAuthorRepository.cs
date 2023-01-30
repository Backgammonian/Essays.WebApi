using Essays.WebApi.Models;

namespace Essays.WebApi.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<ICollection<Author>> GetAuthors();
        Task<ICollection<Author>> GetAuthors(int pageNumber, int pageSize);
        Task<Author?> GetAuthor(string authorId);
        Task<ICollection<Essay>?> GetEssaysOfAuthor(string authorId);
        Task<ICollection<Country>?> GetCountriesOfAuthor(string authorId);
        Task<bool> DoesAuthorExist(string authorId);
        Task<bool> CreateAuthor(Author author);
        Task<bool> AddCountryOfAuthor(string authorId, string countryAbbreviation);
        Task<bool> RemoveCountryOfAuthor(string authorId, string countryAbbreviation);
        Task<bool> UpdateAuthor(Author author);
        Task<bool> DeleteAuthor(Author author);
        Task<bool> Save();
    }
}
