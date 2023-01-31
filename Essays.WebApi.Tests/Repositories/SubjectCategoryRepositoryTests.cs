using Essays.WebApi.Models;

namespace Essays.WebApi.Tests.Repositories
{
    public class SubjectCategoryRepositoryTests
    {
        private readonly TestDatabaseGenerator _dbGenerator;

        public SubjectCategoryRepositoryTests()
        {
            _dbGenerator = new TestDatabaseGenerator();
        }

        [Fact]
        public async Task SubjectCategoryRepository_GetSubjectCategories_ReturnSuccess()
        {
            var repo = new SubjectCategoryRepository(await _dbGenerator.GetDatabase());

            var result = await repo.GetSubjectCategories();

            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(1);
            result.Should().BeAssignableTo<ICollection<SubjectCategory>>();
        }

        [Fact]
        public async Task SubjectCategoryRepository_GetSubjectCategoriesFromPage_ReturnSuccess()
        {
            var repo = new SubjectCategoryRepository(await _dbGenerator.GetDatabase());
            var page = 1;
            var size = 2;

            var result = await repo.GetSubjectCategories(page, size);

            result.Should().NotBeNull();
            result.Should().HaveCountLessThanOrEqualTo(size);
            result.Should().BeAssignableTo<ICollection<SubjectCategory>>();
        }

        [Fact]
        public async Task SubjectCategoryRepository_GetSubjectCategory_ReturnSuccess()
        {
            var repo = new SubjectCategoryRepository(await _dbGenerator.GetDatabase());
            var subjectCategoryId = "1";

            var result = await repo.GetSubjectCategory(subjectCategoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<SubjectCategory>();
        }

        [Fact]
        public async Task SubjectCategoryRepository_GetSubjectsFromCategory_ReturnSuccess()
        {
            var repo = new SubjectCategoryRepository(await _dbGenerator.GetDatabase());
            var subjectCategoryId = "1";

            var result = await repo.GetSubjectsFromCategory(subjectCategoryId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ICollection<Subject>>();
        }

        [Fact]
        public async Task SubjectCategoryRepository_DoesSubjectCategoryExist_ReturnSuccess()
        {
            var repo = new SubjectCategoryRepository(await _dbGenerator.GetDatabase());
            var subjectCategoryId = "1";

            var result = await repo.DoesSubjectCategoryExist(subjectCategoryId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task SubjectCategoryRepository_CreateSubjectCategory_ReturnSuccess()
        {
            var repo = new SubjectCategoryRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetSubjectCategories()).Count;
            var subjectCategory = new SubjectCategory()
            {
                SubjectCategoryId = "10",
                Name = "New subject category"
            };

            var result = await repo.CreateSubjectCategory(subjectCategory);
            var newCount = (await repo.GetSubjectCategories()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount + 1);
        }

        [Fact]
        public async Task SubjectCategoryRepository_UpdateSubjectCategory_ReturnSuccess()
        {
            var dbContext = await _dbGenerator.GetDatabase();
            var repo = new SubjectCategoryRepository(dbContext);
            var subjectCategory = new SubjectCategory()
            {
                SubjectCategoryId = "1",
                Name = "Completely new subject category name"
            };

            //this line somehow prevents unit-test from failing
            dbContext.ChangeTracker.Clear();

            var result = await repo.UpdateSubjectCategory(subjectCategory);
            var changedEntity = await repo.GetSubjectCategory(subjectCategory.SubjectCategoryId);

            result.Should().BeTrue();
            changedEntity.Name.Should().Be(subjectCategory.Name);
        }

        [Fact]
        public async Task SubjectCategoryRepository_DeleteSubjectCategory_ReturnSuccess()
        {
            var dbContext = await _dbGenerator.GetDatabase();
            var repo = new SubjectCategoryRepository(dbContext);
            var oldCount = (await repo.GetSubjectCategories()).Count;
            var subjectCategory = await repo.GetSubjectCategory("1");

            //this line somehow prevents unit-test from failing
            dbContext.ChangeTracker.Clear();

            var result = await repo.DeleteSubjectCategory(subjectCategory);
            var newCount = (await repo.GetSubjectCategories()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount - 1);
        }
    }
}
