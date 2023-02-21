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
    public class SubjectController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IRandomGenerator _randomGenerator;

        public SubjectController(IMapper mapper,
            ISubjectRepository subjectRepository,
            IRandomGenerator randomGenerator)
        {
            _mapper = mapper;
            _subjectRepository = subjectRepository;
            _randomGenerator = randomGenerator;
        }

        [HttpGet("GetSubjects")]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectDto>))]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _subjectRepository.GetSubjects();
            var subjectsDto = _mapper.Map<List<SubjectDto>>(subjects);

            return Ok(subjectsDto);
        }

        [HttpGet("GetSubjectsFromPage")]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectDto>))]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetSubjects([FromQuery] int pageNumber, int pageSize)
        {
            if (pageNumber < 1 ||
                pageSize < 1)
            {
                return StatusCode(422, $"Wrong page '{pageNumber}' of size '{pageSize}'");
            }

            var subjects = await _subjectRepository.GetSubjects(pageNumber, pageSize);
            var subjectsDto = _mapper.Map<List<SubjectDto>>(subjects);

            return Ok(subjectsDto);
        }

        [HttpGet("GetSubject")]
        [ProducesResponseType(200, Type = typeof(SubjectDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubject([FromQuery] string subjectId)
        {
            var any = await _subjectRepository.DoesSubjectExist(subjectId);
            if (!any)
            {
                return NotFound("Subject with such ID doesn't exist");
            }

            var subject = await _subjectRepository.GetSubject(subjectId);
            var subjectDto = _mapper.Map<SubjectDto>(subject);

            return Ok(subjectDto);
        }

        [HttpGet("GetCategoryOfSubject")]
        [ProducesResponseType(200, Type = typeof(SubjectCategoryDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategoryOfSubject([FromQuery] string subjectId)
        {
            var category = await _subjectRepository.GetCategoryOfSubject(subjectId);
            if (category == null)
            {
                return NotFound("Subject with such ID doesn't exist");
            }

            var categoryDto = _mapper.Map<SubjectCategoryDto>(category);

            return Ok(categoryDto);
        }

        [HttpGet("GetEssaysAboutSubject")]
        [ProducesResponseType(200, Type = typeof(ICollection<EssayDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEssaysAboutSubject([FromQuery] string subjectId)
        {
            var essays = await _subjectRepository.GetEssaysAboutSubject(subjectId);
            if (essays == null)
            {
                return NotFound($"There are no essays about subject with ID '{subjectId}'");
            }

            var essaysDto = _mapper.Map<ICollection<EssayDto>>(essays);

            return Ok(essaysDto);
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDto subjectCreate)
        {
            if (subjectCreate == null)
            {
                return BadRequest("Subject model is null!");
            }

            var subjects = await _subjectRepository.GetSubjects();
            var existingSubject = subjects
                .Where(s => s.Name.ToLower() == subjectCreate.Name.ToLower())
                .FirstOrDefault();

            if (existingSubject != null)
            {
                return StatusCode(422, $"Subject with name '{subjectCreate.Name}' already exists");
            }

            var subject = _mapper.Map<Subject>(subjectCreate);
            subject.SubjectId = _randomGenerator.GetRandomId();

            var created = await _subjectRepository.CreateSubject(subject);
            if (!created)
            {
                return BadRequest("Failed to create a new subject");
            }

            return Ok(subject.SubjectId);
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSubject([FromBody] SubjectDto subjectUpdate)
        {
            if (subjectUpdate == null)
            {
                return BadRequest("Subject model is null!");
            }

            var subjectId = subjectUpdate.SubjectId;

            var subject = await _subjectRepository.GetSubjectTracking(subjectId);
            if (subject == null)
            {
                return NotFound("Subject with such ID doesn't exist");
            }

            subject.Name = subjectUpdate.Name;
            subject.Description = subjectUpdate.Description;

            var updated = await _subjectRepository.UpdateSubject(subject);
            if (!updated)
            {
                return StatusCode(500, $"Failed to update the subject with ID '{subjectUpdate.SubjectId}'");
            }

            return Ok(subject.SubjectId);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubject([FromQuery] string subjectId)
        {
            var subjectToDelete = await _subjectRepository.GetSubjectTracking(subjectId);
            if (subjectToDelete == null)
            {
                return NotFound("Subject with such ID doesn't exist");
            }

            var deleted = await _subjectRepository.DeleteSubject(subjectToDelete);
            if (!deleted)
            {
                return StatusCode(500, $"Failed to delete the subject with ID '{subjectId}'");
            }

            return Ok(subjectToDelete.SubjectId);
        }
    }
}
