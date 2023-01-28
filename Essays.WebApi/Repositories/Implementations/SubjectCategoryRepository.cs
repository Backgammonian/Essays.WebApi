using Essays.WebApi.Data;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Essays.WebApi.Repositories.Implementations
{
    public class SubjectCategoryRepository : ISubjectCategoryRepository
    {
        private readonly DataContext _dataContext;

        public SubjectCategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<SubjectCategory>> GetSubjectCategories()
        {
            return await _dataContext.SubjectCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SubjectCategory?> GetSubjectCategory(string subjectCategoryId)
        {
            return await _dataContext.SubjectCategories
                .AsNoTracking()
                .Where(s => s.SubjectCategoryId == subjectCategoryId)
                .Include(s => s.Subjects)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DoesSubjectCategoryExist(string subjectCategoryId)
        {
            return await _dataContext.SubjectCategories
                .AnyAsync(x => x.SubjectCategoryId == subjectCategoryId);
        }

        public async Task<bool> CreateSubjectCategory(SubjectCategory subjectCategory)
        {
            await _dataContext.SubjectCategories.AddAsync(subjectCategory);
            return await Save();
        }

        public async Task<bool> UpdateSubjectCategory(SubjectCategory subjectCategory)
        {
            _dataContext.SubjectCategories.Update(subjectCategory);
            return await Save();
        }

        public async Task<bool> DeleteSubjectCategory(SubjectCategory subjectCategory)
        {
            _dataContext.SubjectCategories.Remove(subjectCategory);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
