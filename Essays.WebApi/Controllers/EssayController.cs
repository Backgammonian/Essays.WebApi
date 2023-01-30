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
    public class EssayController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IEssayRepository _essayRepository;
        private readonly ISubjectRepository _subjectRepository;

        public EssayController(IMapper mapper,
            IRandomGenerator randomGenerator,
            IEssayRepository essayRepository,
            ISubjectRepository subjectRepository)
        {
            _mapper = mapper;
            _randomGenerator = randomGenerator;
            _essayRepository = essayRepository;
            _subjectRepository = subjectRepository;
        }

        [HttpGet("GetEssays")]
        [ProducesResponseType(200, Type = typeof(ICollection<EssayDto>))]
        public async Task<IActionResult> GetEssays()
        {
            var essays = await _essayRepository.GetEssays();
            var essaysDto = _mapper.Map<List<EssayDto>>(essays);

            return Ok(essaysDto);
        }

        [HttpGet("GetEssaysSlice")]
        [ProducesResponseType(200, Type = typeof(ICollection<EssayDto>))]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetEssaysSlice([FromQuery] int pageNumber, int pageSize)
        {
            if (pageNumber < 1 ||
                pageSize < 1)
            {
                return StatusCode(422, $"Wrong page '{pageNumber}' of size '{pageSize}'");
            }

            var essays = await _essayRepository.GetEssays(pageNumber, pageSize);
            var essaysDto = _mapper.Map<List<EssayDto>>(essays);

            return Ok(essaysDto);
        }

        [HttpGet("GetEssay")]
        [ProducesResponseType(200, Type = typeof(EssayDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEssay([FromQuery] string essayId)
        {
            var any = await _essayRepository.DoesEssayExist(essayId);
            if (!any)
            {
                return NotFound("Such essay doesn't exist");
            }

            var essay = await _essayRepository.GetEssay(essayId);
            var essayDto = _mapper.Map<EssayDto>(essay);

            return Ok(essayDto);
        }

        [HttpGet("GetSubjectsOfEssay")]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubjectsOfEssay([FromQuery] string essayId)
        {
            var subjects = await _essayRepository.GetSubjectsOfEssay(essayId);
            if (subjects == null)
            {
                return NotFound($"There are no essays from author with ID '{essayId}'");
            }

            var subjectsDto = _mapper.Map<ICollection<SubjectDto>>(subjects);

            return Ok(subjectsDto);
        }

        [HttpGet("GetAuthorOfEssay")]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAuthorOfEssay([FromQuery] string essayId)
        {
            var author = await _essayRepository.GetAuthorOfEssay(essayId);
            if (author == null)
            {
                return NotFound("Essay with such ID doesn't exist");
            }

            var authorDto = _mapper.Map<AuthorDto>(author);

            return Ok(authorDto);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEssay([FromBody] EssayDto essayCreate)
        {
            if (essayCreate == null)
            {
                return BadRequest("Essay model is null!");
            }

            var essay = _mapper.Map<Essay>(essayCreate);
            essay.EssayId = _randomGenerator.GetRandomId();
            essay.Title = essay.Title.Trim();
            essay.Content = essay.Content.Trim();

            var created = await _essayRepository.CreateEssay(essay);
            if (!created)
            {
                return StatusCode(500, "Failed to create a new essay");
            }

            return Ok(essay.EssayId);
        }

        [HttpPost("AddSubjectOfEssay")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSubjectOfEssay([FromQuery] string essayId, string subjectId)
        {
            var doesEssayExist = await _essayRepository.DoesEssayExist(essayId);
            if (!doesEssayExist)
            {
                StatusCode(422, $"Essay with ID '{essayId}' doesn't exist!");
            }

            var doesSubjectExist = await _subjectRepository.DoesSubjectExist(subjectId);
            if (!doesSubjectExist)
            {
                return StatusCode(422, $"Subject with ID '{subjectId}' doesn't exist!");
            }

            var added = await _essayRepository.AddSubjectOfEssay(essayId, subjectId);
            if (!added)
            {
                return StatusCode(500, $"Failed to add the subject '{subjectId}' to essay '{essayId}'");
            }

            return Ok($"{essayId}, {subjectId}");
        }

        [HttpPost("RemoveSubjectOfEssay")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RemoveSubjectOfEssay([FromQuery] string essayId, string subjectId)
        {
            var doesEssayExist = await _essayRepository.DoesEssayExist(essayId);
            if (!doesEssayExist)
            {
                StatusCode(422, $"Essay with ID '{essayId}' doesn't exist!");
            }

            var doesSubjectExist = await _subjectRepository.DoesSubjectExist(subjectId);
            if (!doesSubjectExist)
            {
                return StatusCode(422, $"Subject with ID '{subjectId}' doesn't exist!");
            }

            var removed = await _essayRepository.RemoveSubjectOfEssay(essayId, subjectId);
            if (!removed)
            {
                return StatusCode(500, $"Failed to remove the subject '{subjectId}' from essay '{essayId}'");
            }

            return Ok($"{essayId}, {subjectId}");
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEssay([FromBody] EssayDto essayUpdate)
        {
            if (essayUpdate == null)
            {
                return BadRequest("Essay model is null!");
            }

            var any = await _essayRepository.DoesEssayExist(essayUpdate.EssayId);
            if (!any)
            {
                return NotFound("Such essay doesn't exist");
            }

            var essay = _mapper.Map<Essay>(essayUpdate);
            essay.Title = essay.Title.Trim();
            essay.Content = essay.Content.Trim();

            var updated = await _essayRepository.UpdateEssay(essay);
            if (!updated)
            {
                return StatusCode(500, $"Failed to update the essay with ID '{essay.EssayId}'");
            }

            return Ok(essay.EssayId);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEssay([FromQuery] string essayId)
        {
            var essayToDelete = await _essayRepository.GetEssay(essayId);
            if (essayToDelete == null)
            {
                return NotFound("Such essay doesn't exist");
            }

            var deleted = await _essayRepository.DeleteEssay(essayToDelete);
            if (!deleted)
            {
                return StatusCode(500, $"Failed to delete the essay with ID '{essayId}'");
            }

            return Ok(essayToDelete.EssayId);
        }
    }
}
