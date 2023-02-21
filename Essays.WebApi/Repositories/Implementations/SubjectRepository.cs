using Essays.WebApi.Data;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Repositories.Implementations
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DataContext _dataContext;

        public SubjectRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Subject>> GetSubjects()
        {
            return await _dataContext.Subjects
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Subject>> GetSubjects(int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _dataContext.Subjects
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Subject?> GetSubject(string subjectId)
        {
            return await _dataContext.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SubjectId == subjectId);
        }

        public async Task<Subject?> GetSubjectTracking(string subjectId)
        {
            return await _dataContext.Subjects
                .FirstOrDefaultAsync(s => s.SubjectId == subjectId);
        }

        public async Task<SubjectCategory?> GetCategoryOfSubject(string subjectId)
        {
            var any = await DoesSubjectExist(subjectId);
            if (!any)
            {
                return null;
            }

            var subject = await _dataContext.Subjects
                .AsNoTracking()
                .Where(s => s.SubjectId == subjectId)
                .Include(s => s.Category)
                .FirstOrDefaultAsync();

            if (subject == null)
            {
                return null;
            }

            return subject.Category;
        }

        public async Task<ICollection<Essay>?> GetEssaysAboutSubject(string subjectId)
        {
            var any = await DoesSubjectExist(subjectId);
            if (!any)
            {
                return null;
            }

            var essaysAboutSubject = await _dataContext.EssaysAboutSubjects
                .AsNoTracking()
                .Where(x => x.SubjectId == subjectId)
                .ToListAsync();

            var essays = new List<Essay>();
            foreach (var essayAboutSubject in essaysAboutSubject)
            {
                var essay = await _dataContext.Essays
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EssayId == essayAboutSubject.EssayId);

                if (essay != null)
                {
                    essays.Add(essay);
                }
            }

            return essays;
        }

        public async Task<bool> DoesSubjectExist(string subjectId)
        {
            return await _dataContext.Subjects
                .AsNoTracking()
                .AnyAsync(x => x.SubjectId == subjectId);
        }

        public async Task<bool> CreateSubject(Subject subject)
        {
            await _dataContext.Subjects.AddAsync(subject);

            return await Save();
        }

        public async Task<bool> UpdateSubject(Subject subject)
        {
            _dataContext.Subjects.Update(subject);

            return await Save();
        }

        public async Task<bool> DeleteSubject(Subject subject)
        {
            _dataContext.Subjects.Remove(subject);

            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}