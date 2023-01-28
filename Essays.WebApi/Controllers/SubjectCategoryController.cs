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
    public class SubjectCategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISubjectCategoryRepository _subjectCategoryRepository;
        private readonly IRandomGenerator _randomGenerator;

        public SubjectCategoryController(IMapper mapper,
            ISubjectCategoryRepository subjectCategoryRepository,
            IRandomGenerator randomGenerator)
        {
            _mapper = mapper;
            _subjectCategoryRepository = subjectCategoryRepository;
            _randomGenerator = randomGenerator;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SubjectCategory>))]
        public async Task<IActionResult> GetSubjectCategories()
        {
            var subjectCategories = await _subjectCategoryRepository.GetSubjectCategories();
            var subjectCategoriesDto = _mapper.Map<List<SubjectCategoryDto>>(subjectCategories);

            return Ok(subjectCategoriesDto);
        }

        [HttpGet("{subjectCategoryId}")]
        [ProducesResponseType(200, Type = typeof(SubjectCategory))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSubjectCategory([FromQuery] string subjectCategoryId)
        {
            var any = await _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId);
            if (!any)
            {
                return NotFound();
            }

            var subjectInfo = await _subjectCategoryRepository.GetSubjectCategory(subjectCategoryId);
            var subjectInfoDto = _mapper.Map<SubjectCategoryDto>(subjectInfo);

            return Ok(subjectInfoDto);
        }

        [HttpGet("{subjectCategoryId}/subjects")]
        [ProducesResponseType(200, Type = typeof(ICollection<Subject>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetSubjectsFromCategory([FromQuery] string subjectCategoryId)
        {
            var any = await _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId);
            if (!any)
            {
                return NotFound();
            }

            var subjectInfo = await _subjectCategoryRepository.GetSubjectCategory(subjectCategoryId);
            if (subjectInfo == null)
            {
                return NoContent();
            }

            var subjectsDto = _mapper.Map<List<SubjectDto>>(subjectInfo.Subjects);

            return Ok(subjectsDto);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSubjectCategory([FromBody] SubjectCategoryDto subjectCategoryCreate)
        {
            if (subjectCategoryCreate == null)
            {
                return BadRequest();
            }

            var subjectCategories = await _subjectCategoryRepository.GetSubjectCategories();
            var existingSubjectCategory = subjectCategories
                .Where(sc => sc.Name.Trim().ToLower() == subjectCategoryCreate.Name.Trim().ToLower())
                .FirstOrDefault();

            if (existingSubjectCategory != null)
            {
                return StatusCode(422);
            }

            var subjectCategory = _mapper.Map<SubjectCategory>(subjectCategoryCreate);
            subjectCategory.Name = subjectCategory.Name.Trim();
            var created = await _subjectCategoryRepository.CreateSubjectCategory(subjectCategory);
            if (!created)
            {
                return StatusCode(500);
            }

            return Ok($"Successfully created new subject category '{subjectCategory.Name}'");
        }

        [HttpPut("{subjectCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateSubjectCategory([FromQuery] string subjectCategoryId,
            [FromBody] SubjectCategoryDto subjectCategoryUpdate)
        {
            if (subjectCategoryUpdate == null)
            {
                return BadRequest();
            }

            if (subjectCategoryId != subjectCategoryUpdate.SubjectCategoryId)
            {
                return BadRequest();
            }

            var any = await _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId);
            if (!any)
            {
                return NotFound();
            }

            var subjectCategory = _mapper.Map<SubjectCategory>(subjectCategoryUpdate);
            subjectCategory.Name = subjectCategory.Name.Trim();
            var updated = await _subjectCategoryRepository.UpdateSubjectCategory(subjectCategory);
            if (!updated)
            {
                return StatusCode(500);
            }

            return Ok($"Successfully updated the subject category '{subjectCategory.Name}'");
        }

        [HttpDelete("{subjectCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSubjectCategory([FromQuery] string subjectCategoryId)
        {
            var any = await _subjectCategoryRepository.DoesSubjectCategoryExist(subjectCategoryId);
            if (!any)
            {
                return NotFound();
            }

            var subjectCategoryToDelete = await _subjectCategoryRepository.GetSubjectCategory(subjectCategoryId);
            if (subjectCategoryToDelete == null)
            {
                return NotFound();
            }

            var deleted = await _subjectCategoryRepository.DeleteSubjectCategory(subjectCategoryToDelete);
            if (!deleted)
            {
                return StatusCode(500);
            }

            return Ok($"Successfully deleted the subject category '{subjectCategoryToDelete.Name}'");
        }
    }
}
