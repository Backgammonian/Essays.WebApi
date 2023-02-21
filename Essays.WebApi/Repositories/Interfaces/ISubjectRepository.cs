using Essays.WebApi.Models;

namespace Essays.WebApi.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<ICollection<Subject>> GetSubjects();
        Task<ICollection<Subject>> GetSubjects(int pageNumber, int pageSize);
        Task<Subject?> GetSubject(string subjectId);
        Task<Subject?> GetSubjectTracking(string subjectId);
        Task<SubjectCategory?> GetCategoryOfSubject(string subjectId);
        Task<ICollection<Essay>?> GetEssaysAboutSubject(string subjectId);
        Task<bool> DoesSubjectExist(string subjectId);
        Task<bool> CreateSubject(Subject subject);
        Task<bool> UpdateSubject(Subject subject);
        Task<bool> DeleteSubject(Subject subject);
        Task<bool> Save();
    }
}
