using AutoMapper;
using Essays.WebApi.Data.Interfaces;
using Essays.WebApi.DTOs;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Essays.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICountryRepository _countryRepository;

        public AuthorController(IMapper mapper,
            IRandomGenerator randomGenerator,
            IAuthorRepository authorRepository,
            ICountryRepository countryRepository)
        {
            _mapper = mapper;
            _randomGenerator = randomGenerator;
            _authorRepository = authorRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet("GetAuthors")]
        [ProducesResponseType(200, Type = typeof(ICollection<AuthorDto>))]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorRepository.GetAuthors();
            var authorsDto = _mapper.Map<List<AuthorDto>>(authors);

            return Ok(authorsDto);
        }

        [HttpGet("GetAuthor")]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAuthor([FromQuery] string authorId)
        {
            var any = await _authorRepository.DoesAuthorExist(authorId);
            if (!any)
            {
                return NotFound("Such author doesn't exist");
            }

            var author = await _authorRepository.GetAuthor(authorId);
            var authorDto = _mapper.Map<AuthorDto>(author);

            return Ok(authorDto);
        }

        [HttpGet("GetEssaysOfAuthor")]
        [ProducesResponseType(200, Type = typeof(ICollection<EssayDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEssaysOfAuthor([FromQuery] string authorId)
        {
            var essays = await _authorRepository.GetEssaysOfAuthor(authorId);
            if (essays == null)
            {
                return NotFound($"There are no essays from author with ID '{authorId}'");
            }

            var essaysDto = _mapper.Map<ICollection<EssayDto>>(essays);

            return Ok(essaysDto);
        }

        [HttpGet("GetCountriesOfAuthor")]
        [ProducesResponseType(200, Type = typeof(ICollection<CountryDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCountriesOfAuthor([FromQuery] string authorId)
        {
            var countries = await _authorRepository.GetCountriesOfAuthor(authorId);
            if (countries == null)
            {
                return NotFound($"Can't find author with ID '{authorId}'");
            }

            var countriesDto = _mapper.Map<ICollection<CountryDto>>(countries);

            return Ok(countriesDto);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto authorCreate)
        {
            if (authorCreate == null)
            {
                return BadRequest("Author model is null!");
            }

            var author = _mapper.Map<Author>(authorCreate);
            author.AuthorId = _randomGenerator.GetRandomId();
            author.FirstName = author.FirstName.Trim();
            author.LastName = author.LastName.Trim();

            var created = await _authorRepository.CreateAuthor(author);
            if (!created)
            {
                return StatusCode(500, "Failed to create a new country");
            }

            return Ok(author.AuthorId);
        }

        [HttpPost("AddCountryOfAuthor")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddCountryOfAuthor([FromQuery] string authorId, string countryAbbreviation)
        {
            var doesAuthorExist = await _authorRepository.DoesAuthorExist(authorId);
            if (!doesAuthorExist)
            {
                StatusCode(422, $"Author with ID '{authorId}' doesn't exist!");
            }

            var doesCountryExist = await _countryRepository.DoesCountryExist(countryAbbreviation);
            if (!doesCountryExist)
            {
                return StatusCode(422, $"Country with abbreviation '{countryAbbreviation}' doesn't exist!");
            }

            var added = await _authorRepository.AddCountryOfAuthor(authorId, countryAbbreviation);
            if (!added)
            {
                return StatusCode(500, $"Failed to add the country '{countryAbbreviation}' to author with ID '{authorId}'");
            }

            return Ok($"{authorId}, {countryAbbreviation}");
        }

        [HttpPost("RemoveCountryOfAuthor")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RemoveCountryOfAuthor([FromQuery] string authorId, string countryAbbreviation)
        {
            var doesAuthorExist = await _authorRepository.DoesAuthorExist(authorId);
            if (!doesAuthorExist)
            {
                StatusCode(422, $"Author with ID '{authorId}' doesn't exist!");
            }

            var doesCountryExist = await _countryRepository.DoesCountryExist(countryAbbreviation);
            if (!doesCountryExist)
            {
                return StatusCode(422, $"Country with abbreviation '{countryAbbreviation}' doesn't exist!");
            }

            var removed = await _authorRepository.RemoveCountryOfAuthor(authorId, countryAbbreviation);
            if (!removed)
            {
                return StatusCode(500, $"Failed to remove the country '{countryAbbreviation}' from author with ID '{authorId}'");
            }

            return Ok($"{authorId}, {countryAbbreviation}");
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorDto authorUpdate)
        {
            if (authorUpdate == null)
            {
                return BadRequest("Author model is null!");
            }

            var any = await _authorRepository.DoesAuthorExist(authorUpdate.AuthorId);
            if (!any)
            {
                return NotFound("Such author doesn't exist");
            }

            var author = _mapper.Map<Author>(authorUpdate);
            author.FirstName = author.FirstName.Trim();
            author.LastName = author.LastName.Trim();

            var updated = await _authorRepository.UpdateAuthor(author);
            if (!updated)
            {
                return StatusCode(500, $"Failed to update the author with ID '{author.AuthorId}'");
            }

            return Ok(author.AuthorId);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAuthor([FromQuery] string authorId)
        {
            var authorToDelete = await _authorRepository.GetAuthor(authorId);
            if (authorToDelete == null)
            {
                return NotFound("Such author doesn't exist");
            }

            var deleted = await _authorRepository.DeleteAuthor(authorToDelete);
            if (!deleted)
            {
                return StatusCode(500, $"Failed to delete the author with ID '{authorId}'");
            }

            return Ok(authorToDelete.AuthorId);
        }
    }
}
