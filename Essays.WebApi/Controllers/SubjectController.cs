using AutoMapper;
using Essays.WebApi.Data.Interfaces;
using Essays.WebApi.DTOs;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Implementations;
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Subject>))]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _subjectRepository.GetSubjects();
            var subjectsDto = _mapper.Map<List<SubjectDto>>(subjects);

            return Ok(subjectsDto);
        }

        [HttpGet("{subjectId}")]
        [ProducesResponseType(200, Type = typeof(Subject))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubject([FromQuery] string subjectId)
        {
            var any = await _subjectRepository.DoesSubjectExist(subjectId);
            if (!any)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetSubject(subjectId);
            var subjectDto = _mapper.Map<SubjectCategoryDto>(subject);

            return Ok(subjectDto);
        }

        [HttpGet("{subjectId}/category")]
        [ProducesResponseType(200, Type = typeof(SubjectCategory))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategoryOfSubject([FromQuery] string subjectId)
        {
            var category = await _subjectRepository.GetCategoryOfSubject(subjectId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("{subjectId}/essays")]
        [ProducesResponseType(200, Type = typeof(ICollection<Essay>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEssaysAboutSubject([FromQuery] string subjectId)
        {
            var essays = await _subjectRepository.GetEssaysAboutSubject(subjectId);
            if (essays == null)
            {
                return NotFound();
            }

            return Ok(essays);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDto subjectCreate)
        {
            if (subjectCreate == null)
            {
                return BadRequest();
            }

            var subjects = await _subjectRepository.GetSubjects();
            var existingSubject = subjects
                .Where(s => s.Name.Trim().ToLower() == subjectCreate.Name.Trim().ToLower())
                .FirstOrDefault();

            if (existingSubject != null)
            {
                return StatusCode(422);
            }

            var subject = _mapper.Map<Subject>(subjectCreate);
            subject.SubjectId = _randomGenerator.GetRandomId();
            subject.Name = subject.Name.Trim();

            var created = await _subjectRepository.CreateSubject(subject);
            if (!created)
            {
                return StatusCode(500);
            }

            return Ok(subject.SubjectId);
        }

        [HttpPut("{subjectId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSubjectCategory([FromQuery] string subjectId,
            [FromBody] SubjectDto subjectUpdate)
        {
            if (subjectUpdate == null)
            {
                return BadRequest();
            }

            if (subjectId != subjectUpdate.SubjectId)
            {
                return BadRequest();
            }

            var any = await _subjectRepository.DoesSubjectExist(subjectId);
            if (!any)
            {
                return NotFound();
            }

            var subject = _mapper.Map<Subject>(subjectUpdate);
            subject.Name = subject.Name.Trim();

            var updated = await _subjectRepository.UpdateSubject(subject);
            if (!updated)
            {
                return StatusCode(500);
            }

            return Ok(subject.SubjectId);
        }

        [HttpDelete("{subjectId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubjectCategory([FromQuery] string subjectId)
        {
            var any = await _subjectRepository.DoesSubjectExist(subjectId);
            if (!any)
            {
                return NotFound();
            }

            var subjectToDelete = await _subjectRepository.GetSubject(subjectId);
            if (subjectToDelete == null)
            {
                return NotFound();
            }

            var deleted = await _subjectRepository.DeleteSubject(subjectToDelete);
            if (!deleted)
            {
                return StatusCode(500);
            }

            return Ok(subjectToDelete.SubjectId);
        }
    }
}
