using Essays.WebApi.Models;

namespace Essays.WebApi.Repositories.Interfaces
{
    public interface IEssayRepository
    {
        Task<ICollection<Essay>> GetEssays();
        Task<ICollection<Essay>> GetEssays(int pageNumber, int pageSize);
        Task<Essay?> GetEssay(string essayId);
        Task<ICollection<Subject>?> GetSubjectsOfEssay(string essayId);
        Task<Author?> GetAuthorOfEssay(string essayId);
        Task<bool> DoesEssayExist(string essayId);
        Task<bool> CreateEssay(Essay essay);
        Task<bool> AddSubjectOfEssay(string essayId, string subjectId);
        Task<bool> RemoveSubjectOfEssay(string essayId, string subjectId);
        Task<bool> UpdateEssay(Essay essay);
        Task<bool> DeleteEssay(Essay essay);
        Task<bool> Save();
    }
}
