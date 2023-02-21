using Essays.WebApi.Models;

namespace Essays.WebApi.Repositories.Interfaces
{
    public interface ISubjectCategoryRepository
    {
        Task<ICollection<SubjectCategory>> GetSubjectCategories();
        Task<ICollection<SubjectCategory>> GetSubjectCategories(int pageNumber, int pageSize);
        Task<SubjectCategory?> GetSubjectCategory(string subjectCategoryId);
        Task<SubjectCategory?> GetSubjectCategoryTracking(string subjectCategoryId);
        Task<ICollection<Subject>?> GetSubjectsFromCategory(string subjectCategoryId);
        Task<bool> DoesSubjectCategoryExist(string subjectCategoryId);
        Task<bool> CreateSubjectCategory(SubjectCategory subjectCategory);
        Task<bool> UpdateSubjectCategory(SubjectCategory subjectCategory);
        Task<bool> DeleteSubjectCategory(SubjectCategory subjectCategory);
        Task<bool> Save();
    }
}
