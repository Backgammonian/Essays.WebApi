namespace Essays.WebApi.Tests.Repositories
{
    public class AuthorRepositoryTests
    {
        private readonly TestDatabaseGenerator _dbGenerator;

        public AuthorRepositoryTests()
        {
            _dbGenerator = new TestDatabaseGenerator();
        }

        [Fact]
        public async Task AuthorRepository_GetAuthors_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());

            var result = await repo.GetAuthors();

            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(1);
            result.Should().BeAssignableTo<ICollection<Author>>();
        }

        [Fact]
        public async Task AuthorRepository_GetAuthorsFromPage_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var page = 1;
            var size = 2;

            var result = await repo.GetAuthors(page, size);

            result.Should().NotBeNull();
            result.Should().HaveCountLessThanOrEqualTo(size);
            result.Should().BeAssignableTo<ICollection<Author>>();
        }

        [Fact]
        public async Task AuthorRepository_GetAuthor_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var authorId = "1";

            var result = await repo.GetAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Author>();
        }

        [Fact]
        public async Task AuthorRepository_GetEssaysOfAuthor_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var authorId = "1";

            var result = await repo.GetEssaysOfAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ICollection<Essay>>();
        }

        [Fact]
        public async Task AuthorRepository_GetCountriesOfAuthor_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var authorId = "1";

            var result = await repo.GetCountriesOfAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ICollection<Country>>();
        }

        [Fact]
        public async Task AuthorRepository_DoesAuthorExist_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var authorId = "1";

            var result = await repo.DoesAuthorExist(authorId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task AuthorRepository_CreateAuthor_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetAuthors()).Count;
            var author = new Author()
            {
                AuthorId = "5",
                FirstName = "First name",
                LastName = "Last name"
            };

            var result = await repo.CreateAuthor(author);
            var newCount = (await repo.GetAuthors()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount + 1);
        }

        [Fact]
        public async Task AuthorRepository_AddCountryOfAuthor_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var authorId = "1";
            var countryAbbreviation = "tf2";

            var result = await repo.AddCountryOfAuthor(authorId, countryAbbreviation);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task AuthorRepository_RemoveCountryOfAuthor_ReturnSuccess()
        {
            var repo = new AuthorRepository(await _dbGenerator.GetDatabase());
            var authorId = "1";
            var countryAbbreviation = "cto";

            var result = await repo.RemoveCountryOfAuthor(authorId, countryAbbreviation);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task AuthorRepository_UpdateAuthor_ReturnSuccess()
        {
            var dbContext = await _dbGenerator.GetDatabase();
            var repo = new AuthorRepository(dbContext);
            var author = new Author()
            {
                AuthorId = "1",
                FirstName = "Completely new first name",
                LastName = "Completely new last name"
            };

            //this line somehow prevents unit-test from failing
            dbContext.ChangeTracker.Clear();

            var result = await repo.UpdateAuthor(author);
            var changedEntity = await repo.GetAuthor(author.AuthorId);

            result.Should().BeTrue();
            changedEntity.FirstName.Should().Be(author.FirstName);
            changedEntity.LastName.Should().Be(author.LastName);
        }

        [Fact]
        public async Task AuthorRepository_DeleteAuthor_ReturnSuccess()
        {
            var dbContext = await _dbGenerator.GetDatabase();
            var repo = new AuthorRepository(dbContext);
            var oldCount = (await repo.GetAuthors()).Count;
            var author = await repo.GetAuthor("1");

            //this line somehow prevents unit-test from failing
            dbContext.ChangeTracker.Clear();

            var result = await repo.DeleteAuthor(author);
            var newCount = (await repo.GetAuthors()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount - 1);
        }
    }
}
