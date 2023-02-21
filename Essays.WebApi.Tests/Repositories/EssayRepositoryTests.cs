namespace Essays.WebApi.Tests.Repositories
{
    public class EssayRepositoryTests
    {
        private readonly TestDatabaseGenerator _dbGenerator;

        public EssayRepositoryTests()
        {
            _dbGenerator = new TestDatabaseGenerator();
        }

        [Fact]
        public async Task EssayRepository_GetEssays_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());

            var result = await repo.GetEssays();

            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(1);
            result.Should().BeAssignableTo<ICollection<Essay>>();
        }

        [Fact]
        public async Task EssayRepository_GetEssaysFromPage_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var page = 1;
            var size = 2;

            var result = await repo.GetEssays(page, size);

            result.Should().NotBeNull();
            result.Should().HaveCountLessThanOrEqualTo(size);
            result.Should().BeAssignableTo<ICollection<Essay>>();
        }

        [Fact]
        public async Task EssayRepository_GetEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";

            var result = await repo.GetEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Essay>();
        }

        [Fact]
        public async Task EssayRepository_GetSubjectsOfEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";

            var result = await repo.GetSubjectsOfEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ICollection<Subject>>();
        }

        [Fact]
        public async Task EssayRepository_GetAuthorOfEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";

            var result = await repo.GetAuthorOfEssay(essayId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Author>();
        }

        [Fact]
        public async Task EssayRepository_DoesEssayExist_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";

            var result = await repo.DoesEssayExist(essayId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task EssayRepository_CreateEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetEssays()).Count;
            var essay = new Essay()
            {
                AuthorId = "1",
                EssayId = "101",
                Title = "New essay title",
                Content = "Lorem ipsum is really outdated, isn't it?"
            };

            var result = await repo.CreateEssay(essay);
            var newCount = (await repo.GetEssays()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount + 1);
        }

        [Fact]
        public async Task EssayRepository_AddSubjectOfEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";
            var subjectId = "3";

            var result = await repo.AddSubjectOfEssay(essayId, subjectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task EssayRepository_RemoveSubjectOfEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";
            var subjectId = "1";

            var result = await repo.RemoveSubjectOfEssay(essayId, subjectId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task EssayRepository_UpdateEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var essayId = "1";
            var essay = await repo.GetEssayTracking(essayId);
            var newTitle = "Completely new essay title";
            var newContent = "Completely new essay content";
            essay.Title = newTitle;
            essay.Content = newContent;

            var result = await repo.UpdateEssay(essay);
            var changedEntity = await repo.GetEssay(essayId);

            result.Should().BeTrue();
            changedEntity.Title.Should().Be(newTitle);
            changedEntity.Content.Should().Be(newContent);
        }

        [Fact]
        public async Task EssayRepository_DeleteEssay_ReturnSuccess()
        {
            var repo = new EssayRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetEssays()).Count;
            var essayId = "1";
            var essay = await repo.GetEssayTracking(essayId);

            var result = await repo.DeleteEssay(essay);
            var newCount = (await repo.GetEssays()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount - 1);
        }
    }
}
