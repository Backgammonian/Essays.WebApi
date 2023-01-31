namespace Essays.WebApi.Tests.Repositories
{
    public class CountryRepositoryTests
    {
        private readonly TestDatabaseGenerator _dbGenerator;

        public CountryRepositoryTests()
        {
            _dbGenerator = new TestDatabaseGenerator();
        }

        [Fact]
        public async Task CountryRepository_GetCountries_ReturnSuccess()
        {
            var repo = new CountryRepository(await _dbGenerator.GetDatabase());

            var result = await repo.GetCountries();

            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(1);
            result.Should().BeAssignableTo<ICollection<Country>>();
        }

        [Fact]
        public async Task CountryRepository_GetCountriesFromPage_ReturnSuccess()
        {
            var repo = new CountryRepository(await _dbGenerator.GetDatabase());
            var page = 1;
            var size = 2;

            var result = await repo.GetCountries(page, size);

            result.Should().NotBeNull();
            result.Should().HaveCountLessThanOrEqualTo(size);
            result.Should().BeAssignableTo<ICollection<Country>>();
        }

        [Fact]
        public async Task CountryRepository_GetCountry_ReturnSuccess()
        {
            var repo = new CountryRepository(await _dbGenerator.GetDatabase());
            var countryAbbreviation = "cto";

            var result = await repo.GetCountry(countryAbbreviation);

            result.Should().NotBeNull();
            result.Should().BeOfType<Country>();
        }

        [Fact]
        public async Task CountryRepository_GetAuthorsFromCountry_ReturnSuccess()
        {
            var repo = new CountryRepository(await _dbGenerator.GetDatabase());
            var countryAbbreviation = "cto";

            var result = await repo.GetAuthorsFromCountry(countryAbbreviation);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ICollection<Author>>();
        }

        [Fact]
        public async Task CountryRepository_DoesCountryExist_ReturnSuccess()
        {
            var repo = new CountryRepository(await _dbGenerator.GetDatabase());
            var countryAbbreviation = "cto";

            var result = await repo.DoesCountryExist(countryAbbreviation);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CountryRepository_CreateCountry_ReturnSuccess()
        {
            var repo = new CountryRepository(await _dbGenerator.GetDatabase());
            var oldCount = (await repo.GetCountries()).Count;
            var country = new Country()
            {
                CountryAbbreviation = "tf",
                CountryName = "Teufort"
            };

            var result = await repo.CreateCountry(country);
            var newCount = (await repo.GetCountries()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount + 1);
        }

        [Fact]
        public async Task CountryRepository_UpdateCountry_ReturnSuccess()
        {
            var dbContext = await _dbGenerator.GetDatabase();
            var repo = new CountryRepository(dbContext);
            var country = new Country()
            {
                CountryAbbreviation = "cto",
                CountryName = "Teufort"
            };

            //this line somehow prevents unit-test from failing
            dbContext.ChangeTracker.Clear();

            var result = await repo.UpdateCountry(country);
            var changedEntity = await repo.GetCountry(country.CountryAbbreviation);

            result.Should().BeTrue();
            changedEntity.CountryName.Should().Be(country.CountryName);
        }

        [Fact]
        public async Task CountryRepository_DeleteCountry_ReturnSuccess()
        {
            var dbContext = await _dbGenerator.GetDatabase();
            var repo = new CountryRepository(dbContext);
            var oldCount = (await repo.GetCountries()).Count;
            var country = await repo.GetCountry("cto");

            //this line somehow prevents unit-test from failing
            dbContext.ChangeTracker.Clear();

            var result = await repo.DeleteCountry(country);
            var newCount = (await repo.GetCountries()).Count;

            result.Should().BeTrue();
            newCount.Should().Be(oldCount - 1);
        }
    }
}
