﻿using FakeItEasy;

namespace Essays.WebApi.Tests.Controllers
{
    public class CountryControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;
        private readonly CountryController _countryController;

        public CountryControllerTests()
        {
            _mapper = A.Fake<IMapper>();
            _countryRepository = A.Fake<ICountryRepository>();
            _countryController = new CountryController(_mapper,
                _countryRepository);
        }

        [Fact]
        public async Task CountryController_GetCountries_ReturnsOK()
        {
            var countries = A.Fake<ICollection<Country>>();
            var countriesDto = A.Fake<ICollection<CountryDto>>();
            A.CallTo(() => _countryRepository.GetCountries()).Returns(countries);
            A.CallTo(() => _mapper.Map<ICollection<CountryDto>>(countries)).Returns(countriesDto);

            var result = await _countryController.GetCountries();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CountryController_GetCountriesFromPage_ReturnsOK()
        {
            var countries = A.Fake<ICollection<Country>>();
            var countriesDto = A.Fake<ICollection<CountryDto>>();
            var page = 1;
            var size = 10;
            A.CallTo(() => _countryRepository.GetCountries(page, size)).Returns(countries);
            A.CallTo(() => _mapper.Map<ICollection<CountryDto>>(countries)).Returns(countriesDto);

            var result = await _countryController.GetCountries(page, size);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CountryController_GetCountry_ReturnsOK()
        {
            var countryId = "1";
            var country = A.Fake<Country>();
            var countryDto = A.Fake<CountryDto>();
            A.CallTo(() => _countryRepository.DoesCountryExist(countryId)).Returns(true);
            A.CallTo(() => _countryRepository.GetCountry(countryId)).Returns(country);
            A.CallTo(() => _mapper.Map<CountryDto>(country)).Returns(countryDto);

            var result = await _countryController.GetCountry(countryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CountryController_GetAuthorsFromCountry_ReturnsOK()
        {
            var countryAbbreviation = "ctr";
            var authors = A.Fake<ICollection<Author>>();
            var authorsDto = A.Fake<ICollection<AuthorDto>>();
            A.CallTo(() => _countryRepository.GetAuthorsFromCountry(countryAbbreviation)).Returns(authors);
            A.CallTo(() => _mapper.Map<ICollection<AuthorDto>>(authors)).Returns(authorsDto);

            var result = await _countryController.GetAuthorsFromCountry(countryAbbreviation);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CountryController_CreateCountry_ReturnsOK()
        {
            var createCountryDto = A.Fake<CreateCountryDto>();
            var countries = A.Fake<ICollection<Country>>();
            A.CallTo(() => _countryRepository.GetCountries()).Returns(countries);
            A.CallTo(() => _countryRepository.CreateCountry(createCountryDto)).Returns(true);

            var result = await _countryController.CreateCountry(createCountryDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CountryController_UpdateCountry_ReturnsOK()
        {
            var countryDto = A.Fake<CountryDto>();
            var country = A.Fake<Country>();
            A.CallTo(() => _countryRepository.GetCountryTracking(countryDto.CountryAbbreviation)).Returns(country);
            A.CallTo(() => _countryRepository.UpdateCountry(country)).Returns(true);

            var result = await _countryController.UpdateCountry(countryDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CountryController_DeleteCountry_ReturnsOK()
        {
            var countryAbbreviation = "ctr";
            var country = A.Fake<Country>();
            A.CallTo(() => _countryRepository.GetCountryTracking(countryAbbreviation)).Returns(country);
            A.CallTo(() => _countryRepository.DeleteCountry(country)).Returns(true);

            var result = await _countryController.DeleteCountry(countryAbbreviation);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
