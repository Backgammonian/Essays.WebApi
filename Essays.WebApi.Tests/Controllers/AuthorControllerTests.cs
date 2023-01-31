namespace Essays.WebApi.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly AuthorController _authorController;

        public AuthorControllerTests()
        {
            _mapper = A.Fake<IMapper>();
            _randomGenerator = A.Fake<IRandomGenerator>();
            _authorRepository = A.Fake<IAuthorRepository>();
            _countryRepository = A.Fake<ICountryRepository>();
            _authorController = new AuthorController(_mapper,
                _randomGenerator,
                _authorRepository,
                _countryRepository);
        }

        [Fact]
        public async Task AuthorController_GetAuthors_ReturnsOK()
        {
            var authors = A.Fake<ICollection<Author>>();
            var authorsDto = A.Fake<ICollection<AuthorDto>>();
            A.CallTo(() => _authorRepository.GetAuthors()).Returns(authors);
            A.CallTo(() => _mapper.Map<ICollection<AuthorDto>>(authors)).Returns(authorsDto);

            var result = await _authorController.GetAuthors();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_GetAuthorsFromPage_ReturnsOK()
        {
            var authors = A.Fake<ICollection<Author>>();
            var authorsDto = A.Fake<ICollection<AuthorDto>>();
            A.CallTo(() => _authorRepository.GetAuthors()).Returns(authors);
            A.CallTo(() => _mapper.Map<ICollection<AuthorDto>>(authors)).Returns(authorsDto);

            var result = await _authorController.GetAuthors(1, 10);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_GetAuthor_ReturnsOK()
        {
            var authorId = "1";
            var author = A.Fake<Author>();
            var authorDto = A.Fake<AuthorDto>();
            A.CallTo(() => _authorRepository.DoesAuthorExist(authorId)).Returns(true);
            A.CallTo(() => _authorRepository.GetAuthor(authorId)).Returns(author);
            A.CallTo(() => _mapper.Map<AuthorDto>(author)).Returns(authorDto);

            var result = await _authorController.GetAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_GetEssaysOfAuthor_ReturnsOK()
        {
            var authorId = "1";
            var essays = A.Fake<ICollection<Essay>>();
            var essaysDto = A.Fake<ICollection<EssayDto>>();
            A.CallTo(() => _authorRepository.GetEssaysOfAuthor(authorId)).Returns(essays);
            A.CallTo(() => _mapper.Map<ICollection<EssayDto>>(essays)).Returns(essaysDto);

            var result = await _authorController.GetEssaysOfAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_GetCountriesOfAuthor_ReturnsOK()
        {
            var authorId = "1";
            var countries = A.Fake<ICollection<Country>>();
            var countriesDto = A.Fake<ICollection<CountryDto>>();
            A.CallTo(() => _authorRepository.GetCountriesOfAuthor(authorId)).Returns(countries);
            A.CallTo(() => _mapper.Map<ICollection<CountryDto>>(countries)).Returns(countriesDto);

            var result = await _authorController.GetCountriesOfAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_CreateAuthor_ReturnsOK()
        {
            var authorDto = A.Fake<AuthorDto>();
            var author = A.Fake<Author>();
            A.CallTo(() => _mapper.Map<Author>(authorDto)).Returns(author);
            A.CallTo(() => _authorRepository.CreateAuthor(author)).Returns(true);

            var result = await _authorController.CreateAuthor(authorDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_AddCountryOfAuthor_ReturnsOK()
        {
            var authorId = "1";
            var countryAbbreviation = "ctr";
            A.CallTo(() => _authorRepository.DoesAuthorExist(authorId)).Returns(true);
            A.CallTo(() => _countryRepository.DoesCountryExist(countryAbbreviation)).Returns(true);
            A.CallTo(() => _authorRepository.AddCountryOfAuthor(authorId, countryAbbreviation)).Returns(true);

            var result = await _authorController.AddCountryOfAuthor(authorId, countryAbbreviation);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_RemoveCountryOfAuthor_ReturnsOK()
        {
            var authorId = "1";
            var countryAbbreviation = "ctr";
            A.CallTo(() => _authorRepository.DoesAuthorExist(authorId)).Returns(true);
            A.CallTo(() => _countryRepository.DoesCountryExist(countryAbbreviation)).Returns(true);
            A.CallTo(() => _authorRepository.RemoveCountryOfAuthor(authorId, countryAbbreviation)).Returns(true);

            var result = await _authorController.RemoveCountryOfAuthor(authorId, countryAbbreviation);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_UpdateAuthor_ReturnsOK()
        {
            var authorDto = A.Fake<AuthorDto>();
            var author = A.Fake<Author>();
            A.CallTo(() => _authorRepository.DoesAuthorExist(authorDto.AuthorId)).Returns(true);
            A.CallTo(() => _mapper.Map<Author>(authorDto)).Returns(author);
            A.CallTo(() => _authorRepository.UpdateAuthor(author)).Returns(true);

            var result = await _authorController.UpdateAuthor(authorDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthorController_DeleteAuthor_ReturnsOK()
        {
            var authorId = "1";
            var author = A.Fake<Author>();
            A.CallTo(() => _authorRepository.GetAuthor(authorId)).Returns(author);
            A.CallTo(() => _authorRepository.DeleteAuthor(author)).Returns(true);

            var result = await _authorController.DeleteAuthor(authorId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
