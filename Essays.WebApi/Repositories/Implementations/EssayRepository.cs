using Essays.WebApi.Data;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Repositories.Implementations
{
    public class EssayRepository : IEssayRepository
    {
        private readonly DataContext _dataContext;

        public EssayRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<Essay>> GetEssays()
        {
            return await _dataContext.Essays
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Essay>> GetEssays(int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            return await _dataContext.Essays
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Essay?> GetEssay(string essayId)
        {
            return await _dataContext.Essays
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EssayId == essayId);
        }

        public async Task<ICollection<Subject>?> GetSubjectsOfEssay(string essayId)
        {
            var any = await DoesEssayExist(essayId);
            if (!any)
            {
                return null;
            }

            var subjectsOfEssay = await _dataContext.EssaysAboutSubjects
               .AsNoTracking()
               .Where(x => x.EssayId == essayId)
               .ToListAsync();

            var subjects = new List<Subject>();
            foreach (var subjectOfEssay in subjectsOfEssay)
            {
                var subject = await _dataContext.Subjects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SubjectId == subjectOfEssay.SubjectId);

                if (subject != null)
                {
                    subjects.Add(subject);
                }
            }

            return subjects;
        }

        public async Task<Author?> GetAuthorOfEssay(string essayId)
        {
            var any = await DoesEssayExist(essayId);
            if (!any)
            {
                return null;
            }

            var essay = await _dataContext.Essays
                .AsNoTracking()
                .Where(e => e.EssayId == essayId)
                .Include(e => e.Author)
                .FirstOrDefaultAsync();

            if (essay == null)
            {
                return null;
            }

            return essay.Author;
        }

        public async Task<bool> DoesEssayExist(string essayId)
        {
            return await _dataContext.Essays
                .AnyAsync(e => e.EssayId == essayId);
        }

        public async Task<bool> CreateEssay(Essay essay)
        {
            await _dataContext.Essays.AddAsync(essay);
            return await Save();
        }

        public async Task<bool> AddSubjectOfEssay(string essayId, string subjectId)
        {
            var subjectOfEssay = new EssaysAboutSubjects()
            {
                EssayId = essayId,
                SubjectId = subjectId
            };

            await _dataContext.EssaysAboutSubjects.AddAsync(subjectOfEssay);
            return await Save();
        }

        public async Task<bool> RemoveSubjectOfEssay(string essayId, string subjectId)
        {
            var subjectOfEssay = await _dataContext.EssaysAboutSubjects
                .FindAsync(essayId, subjectId);

            if (subjectOfEssay == null)
            {
                return false;
            }

            _dataContext.EssaysAboutSubjects.Remove(subjectOfEssay);
            return await Save();
        }

        public async Task<bool> UpdateEssay(Essay essay)
        {
            _dataContext.Essays.Update(essay);
            return await Save();
        }

        public async Task<bool> DeleteEssay(Essay essay)
        {
            _dataContext.Essays.Remove(essay);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
